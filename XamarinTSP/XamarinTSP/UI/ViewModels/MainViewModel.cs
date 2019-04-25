using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.TSP;
using XamarinTSP.UI.CustomControls;
using XamarinTSP.UI.Models;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private GoogleMapsService _googleMapsService;
        private TSPAlgorithm _tspAlgorithm;
        private TSPConfiguration _tspConfiguration;
        private INavigator _navigator;
        private IGeolocationService _geolocation;
        public bool IsTSPRunning { get; private set; }
        public CustomMapController MapController { get; private set; }
        public LocationList List { get; private set; }

        public MainViewModel(INavigator navigator, IGeolocationService geolocation, CustomMapController mapController, LocationList list, GoogleMapsService googleMapsService)
        {
            List = list;
            MapController = mapController;
            _geolocation = geolocation;
            _googleMapsService = googleMapsService;
            _navigator = navigator;

            _tspConfiguration = new TSPConfiguration();
            _tspAlgorithm = new TSPAlgorithm(_tspConfiguration);
        }

        public ICommand OnAppearingCommand => new Command(() =>
        {
            if (List.Locations.Count == 0)
            {
                List.SetMockData(_geolocation);
            }
        });
        public ICommand SelectCommand => new Command<Location>(async selected =>
        {
            List.SelectedLocation = selected;
            await _navigator.PushAsync<SetLocationViewModel>();
        });
        public ICommand AddLocationCommand => new Command(async () =>
        {
            await _navigator.PushAsync<SetLocationViewModel>();
        });
        public ICommand OpenConfigurationCommand => new Command(async () =>
        {
            await _navigator.PushAsync<SetLocationViewModel>();
        });
        public ICommand OpenInGoogleMapsCommand => new Command(() =>
        {
            _googleMapsService.OpenInGoogleMaps(List.Locations);
        });
        public ICommand RunTSPCommand => new Command<Button>(async button =>
        {
            var finished = Helper.InvokeOnMainThreadAsync(() =>
            {
                IsTSPRunning = false;
                button.Image = new FileImageSource() { File = "ic_pause_black_24dp.png" };
                NotifyOfPropertyChange(() => IsTSPRunning);
            });
            if (IsTSPRunning)
            {
                _tspAlgorithm.Stop();
                await finished;
            }
            IsTSPRunning = true;
            NotifyOfPropertyChange(() => IsTSPRunning);
            button.Image = new FileImageSource() { File = "ic_play_arrow_black_24dp.png" };

            await Task.Run(async () =>
            {
                var dest = List.Locations.Select(x => $"{x.Position.Latitude},{x.Position.Longitude}").ToArray();
                var configuration = new DistanceMatrixRequestConfiguration()
                {
                    Origins = dest,
                    Destinations = dest
                };
                try
                {
                    var response = await _googleMapsService.GetDistanceMatrix(configuration);
                    var data = new DistanceMatrixData(response);

                    Action<Route> displayRoute = new Action<Route>(async route =>
                    {
                        MapController.CalculatedRoute = route;
                        await Helper.InvokeOnMainThreadAsync(() => MapController.DisplayRoute());
                    });
                    
                    _tspAlgorithm.Run(
                        new TSPData(
                            List.Locations,
                            data.DistanceMatrix,
                            data.DurationMatrix,
                            _tspConfiguration.ReturnToOrigin), 
                        displayRoute, 
                        List.Locations.Count * 10);

                }
                catch (Exception ex)
                {
                    await Helper.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("TSP ERROR", ex.Message, "OK"));
                    return;
                }
                finally
                {
                    await finished;
                }
            }).ConfigureAwait(false);

        });
    }
}
