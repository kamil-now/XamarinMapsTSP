using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
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

        public MapViewModel MapViewModel { get; set; }
        public LocationListViewModel LocationListViewModel { get; set; }

        public MainViewModel(MapViewModel mapViewModel, LocationListViewModel locationListViewModel, GoogleMapsService googleMapsService)
        {
            MapViewModel = mapViewModel;
            LocationListViewModel = locationListViewModel;

            _googleMapsService = googleMapsService;

            _tspConfiguration = new TSPConfiguration();
            _tspAlgorithm = new TSPAlgorithm(_tspConfiguration);

            LocationListViewModel.List.Locations.CollectionChanged += mapViewModel.ListChanged;

            LocationListViewModel.List.Locations.Add(new Location() { Name = "Warszawa" }); //!temporary - autosize on empty collection  
            LocationListViewModel.List.Locations.Add(new Location() { Name = "Kraków" });
            LocationListViewModel.List.Locations.Add(new Location() { Name = "szczecin" });
            LocationListViewModel.List.Locations.Add(new Location() { Name = "Kołobrzeg" });
            LocationListViewModel.List.Locations.Add(new Location() { Name = "Wrocław" });
            LocationListViewModel.List.Locations.Add(new Location() { Name = "Gdańsk" });
            LocationListViewModel.List.Locations.Add(new Location() { Name = "Rzeszów" });
        }
        public void DisplayRoute()
        {
            //TODO
        }
        public ICommand OnAppearingCommand => new Command(async () =>
        {
            await MapViewModel.MoveToUserRegion();
        });
        public ICommand RunTSPCommand => new Command(async () =>
        {
            string[] waypoints = LocationListViewModel.List.Locations.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToArray();

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

            _googleMapsService.OpenInGoogleMaps(route);
        });
    }
}
