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
        public CustomMap CustomMap { get; set; }
        public LocationList List { get; set; }

        public MainViewModel(INavigator navigator, IGeolocationService geolocation, LocationList list, GoogleMapsService googleMapsService)
        {
            List = list;
            //temp
            List = MockLocationList.List(geolocation);
            List.Locations.ForEach(x => x.Name = geolocation.GetLocationName(x.Position).Result.FirstOrDefault());
            NotifyOfPropertyChange(() => List.Locations.Count);
            //
            _googleMapsService = googleMapsService;
            _navigator = navigator;

            _tspConfiguration = new TSPConfiguration();
            _tspAlgorithm = new TSPAlgorithm(_tspConfiguration);

            CustomMap = new CustomMap(List, geolocation);
        }
        public void DisplayRoute()
        {
            //TODO
        }
        public ICommand OnAppearingCommand => new Command(async () =>
        {
            await CustomMap.MoveToUserRegion();
        });
        public ICommand AddLocationCommand => new Command(async () =>
        {

            await _navigator.PushAsync<SetLocationViewModel>();
        });
        public ICommand OpenConfigurationCommand => new Command(async () =>
        {

        });
        public ICommand OpenInGoogleMapsCommand => new Command(() =>
        {
            //TODO manage empty name case
            _googleMapsService.OpenInGoogleMaps(List.Locations.Select(x => x.Name).ToArray());
        });
        public ICommand RunTSPCommand => new Command(async () =>
        {
            //TODO manage empty name case
            string[] waypoints = List.Locations.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToArray();

            var configuration = new DistanceMatrixRequestConfiguration()
            {
                Origins = waypoints,
                Destinations = waypoints
            };
            int[] result = null;
            try
            {
                var response = await _googleMapsService.GetDistanceMatrix(configuration);
                var data = new DistanceMatrixData(response);

                result = _tspAlgorithm.Run(new DistanceData(data.DistanceMatrix, _tspConfiguration.ReturnToOrigin), waypoints.Length * 200);

            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("TSP ERROR", ex.Message, "OK");
                return;
            }

            string[] route = new string[_tspConfiguration.ReturnToOrigin ? result.Length + 1 : result.Length];

            for (int i = 0; i < result.Length - 1; i++)
            {
                route[i] = waypoints[result[i]];
            }
            if (_tspConfiguration.ReturnToOrigin)
                route[result.Length] = waypoints[0];

            List.Locations = new System.Collections.ObjectModel.ObservableCollection<Location>(route.Select(x => new Location() { Name = x }));
            //_googleMapsService.OpenInGoogleMaps(route);
        });
    }
}
