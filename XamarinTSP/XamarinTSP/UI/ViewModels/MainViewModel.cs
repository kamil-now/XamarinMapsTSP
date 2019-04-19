using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinTSP.Abstractions;
using XamarinTSP.TSP;
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

        public CustomMap CustomMap { get; set; }
        public LocationList List { get; set; }

        public MainViewModel(INavigator navigator, IGeolocationService geolocation, LocationList list, GoogleMapsService googleMapsService)
        {
            List = list;

            _googleMapsService = googleMapsService;
            _navigator = navigator;
            _geolocation = geolocation;

            _tspConfiguration = new TSPConfiguration();
            _tspAlgorithm = new TSPAlgorithm(_tspConfiguration);
            CustomMap = new CustomMap(List, _geolocation);
        }
        public void DisplayRoute()
        {
            //TODO
        }
        public ICommand OnAppearingCommand => new Command(() =>
        {
            CustomMap.FocusOnPins();
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
            button.Image = new FileImageSource() { File = button.Image.File.Replace("black", "blue_light") };
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

                result = _tspAlgorithm.Run(new DistanceData(data.DistanceMatrix, _tspConfiguration.ReturnToOrigin), List.Locations.Count * 200);

                var route = new List<Location>();
                for (int i = 0; i < result.Length - 1; i++)
                {
                    route.Add(List.Locations.ElementAt(result[i]));
                }
                if (_tspConfiguration.ReturnToOrigin)
                    route.Add(List.Locations.ElementAt(0));
                
                List.Locations = new System.Collections.ObjectModel.ObservableCollection<Location>(route);
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("TSP ERROR", ex.Message, "OK");
                return;
            }
            await Application.Current.MainPage.DisplayAlert("TSP FINISHED", "Optimal route has been calculated", "OK");
        });
    }
}
