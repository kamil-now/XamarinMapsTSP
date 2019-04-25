using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;
using XamarinTSP.TSP;
using XamarinTSP.UI.CustomControls;
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
            if (IsTSPRunning)
                return;
            IsTSPRunning = true;
            NotifyOfPropertyChange(() => IsTSPRunning);
            button.Image = new FileImageSource() { File = button.Image.File.Replace("black", "blue_light") };

            await Task.Run(async () =>
            {
                var dest = List.Locations.Select(x => $"{x.Position.Latitude},{x.Position.Longitude}").ToArray();
                var configuration = new DistanceMatrixRequestConfiguration()
                {
                    Origins = dest,
                    Destinations = dest
                };
                int[] result = null;
                try
                {
                    var response = await _googleMapsService.GetDistanceMatrix(configuration);
                    var data = new DistanceMatrixData(response);

                    var tsp = _tspAlgorithm.Run(new TSPData(data.DistanceMatrix, data.DurationMatrix, _tspConfiguration.ReturnToOrigin), List.Locations.Count * 200);

                    result = tsp.waypoints;

                    var route = new List<Location>();
                    for (int i = 0; i < result.Length; i++)
                    {
                        route.Add(List.Locations.ElementAt(result[i]));
                    }
                    if (_tspConfiguration.ReturnToOrigin)
                        route.Add(List.Locations.ElementAt(0));
                    await Helper.InvokeOnMainThreadAsync(() =>
                    {
                        MapController.DisplayRoute(route.Select(x => x.Position).ToList());
                        MapController.CalculatedRoute.Distance = Distance.FromMeters(Math.Round(tsp.distance / 10));
                        MapController.CalculatedRoute.Time = TimeSpan.FromSeconds(tsp.time);

                        NotifyOfPropertyChange(() => MapController.CalculatedRoute.Time);
                        NotifyOfPropertyChange(() => MapController.CalculatedRoute.Distance);
                    });

                }
                catch (Exception ex)
                {
                    await Helper.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("TSP ERROR", ex.Message, "OK"));
                    return;
                }
                finally
                {
                    await Helper.InvokeOnMainThreadAsync(() =>
                    {
                        IsTSPRunning = false;
                        button.Image = new FileImageSource() { File = button.Image.File.Replace("blue_light", "black") };
                        NotifyOfPropertyChange(() => IsTSPRunning);
                    });

                }
                //await Helper.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("TSP FINISHED", "Optimal route has been calculated", "OK"));
            }).ConfigureAwait(false);

        });
    }
}
