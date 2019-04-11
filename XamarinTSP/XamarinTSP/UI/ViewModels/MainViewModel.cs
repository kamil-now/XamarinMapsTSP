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
        public MapViewModel MapViewModel { get; set; }
        public LocationListViewModel LocationListViewModel { get; set; }
        private GoogleMapsService _googleMapsService;
        public MainViewModel(MapViewModel mapViewModel, LocationListViewModel locationListViewModel, GoogleMapsService googleMapsService)
        {
            MapViewModel = mapViewModel;
            LocationListViewModel = locationListViewModel;
            _googleMapsService = googleMapsService;

            locationListViewModel.Locations.CollectionChanged += mapViewModel.ListChanged;
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
            var returnToOrigin = true;
            string[] waypoints = LocationListViewModel.Locations.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x=>x.Name).ToArray();

            var configuration = new DistanceMatrixConfiguration()
            {
                Origins = waypoints,
                Destinations = waypoints
            };

            var response = await _googleMapsService.GetDistanceMatrix(configuration);
            if (response == null)
            {
                //TODO handle exceptions
                return;
            }
            var data = new DistanceMatrixData(response);

            var result = TSPAlgorithm.RUN(data.DistanceMatrix, waypoints.Length * 200, returnToOrigin);

            string[] route = new string[returnToOrigin ? result.Length + 1 : result.Length];

            for (int i = 0; i < result.Length - 1; i++)
            {
                route[i] = waypoints[result[i]];
            }
            if (returnToOrigin)
                route[result.Length] = waypoints[0];

            _googleMapsService.OpenInGoogleMaps(route);
        });
    }
}
