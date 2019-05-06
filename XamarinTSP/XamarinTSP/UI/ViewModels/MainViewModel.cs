using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.GoogleMapsApi.Abstractions;
using XamarinTSP.GoogleMapsApi.Enums;
using XamarinTSP.TSP.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Models;

namespace XamarinTSP.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static volatile object _lck = new object();

        private IGoogleMapsService _googleMapsService;
        private IRouteGeneticAlgorithm _tspAlgorithm;
        private INavigator _navigator;
        private IGeolocationService _geolocation;
        private TravelMode _travelMode;

        public bool IsTSPRunning { get; private set; }
        public bool RouteCalculated { get; private set; }

        public MapViewModel MapViewModel { get; private set; }
        public LocationList List { get; private set; }

        public TravelMode TravelMode
        {
            get => _travelMode;
            set
            {
                _travelMode = value;
                NotifyOfPropertyChange();
            }
        }

        public MainViewModel(INavigator navigator,
                             IGeolocationService geolocation,
                             IRouteGeneticAlgorithm tspAlgorithm,
                             IGoogleMapsService googleMapsService,
                             MapViewModel mapViewModel,
                             LocationList list)
        {
            List = list;
            MapViewModel = mapViewModel;
            _geolocation = geolocation;
            _googleMapsService = googleMapsService;
            _navigator = navigator;
            _tspAlgorithm = tspAlgorithm;

            _travelMode = TravelMode.Driving;

            List.Locations.CollectionChanged += Locations_CollectionChanged;
        }


        public ICommand OnAppearingCommand => new Command(async () =>
        {
            if (List.Locations.Count == 0)
            {
                List.SetMockData(_geolocation);
            }
            if (List.Locations?.Count == 0)
                await MapViewModel.MoveToUserRegion();
        });

        public ICommand RunTSPCommand => new Command<Button>(async button =>
        {
            if (IsTSPRunning)
            {
                _tspAlgorithm.Stop();
                IsTSPRunning = false;
                NotifyOfPropertyChange(() => IsTSPRunning);
                return;
            }

            IsTSPRunning = true;
            NotifyOfPropertyChange(() => IsTSPRunning);

            MapViewModel.CalculatedRoute = new Route()
            {
                RouteCoordinates = List.Locations.Select(x => x.Position).ToList(),
                Distance = Distance.FromMeters(0),
                Time = TimeSpan.MinValue

            };
            MapViewModel.DisplayRoute();
            await Task.Run(async () =>
            {
                try
                {
                    var data = _googleMapsService.GetDistanceMatrix(List.Locations.Select(x => $"{x.Position.Latitude},{x.Position.Longitude}").ToArray(), TravelMode);


                    _tspAlgorithm.RUN(
                        data,
                        List.Locations as IEnumerable<object>,
                        new Action<IRouteElement, IFitnessFunction>(async (element, tspData) =>
                        {
                            await App.InvokeOnMainThreadAsync(() =>
                            {
                                lock (_lck)
                                {
                                    ReaorderLocations(element.Data);

                                    var route = new Route()
                                    {
                                        RouteCoordinates = List.Locations.Select(x => x.Position).ToList(),
                                        Distance = Distance.FromMeters(Math.Round(element.Value)),
                                        Time = TimeSpan.FromSeconds(element.DurationValue)

                                    };
                                    route.RouteCoordinates.Add(List.Locations.ElementAt(0).Position);

                                    MapViewModel.CalculatedRoute = route;
                                    RouteCalculated = true;


                                    NotifyOfPropertyChange(() => RouteCalculated);
                                    MapViewModel.DisplayRoute();
                                }
                            });
                        }));

                }
                catch (Exception ex)
                {
                    IsTSPRunning = false;
                    await App.InvokeOnMainThreadAsync(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("TSP ERROR", ex.Message, "OK");
                        NotifyOfPropertyChange(() => IsTSPRunning);
                    });
                    return;
                }
            }).ConfigureAwait(false);
        });
        public ICommand SelectLocationCommand => new Command<Location>(selected => MapViewModel.MapPosition = selected.Position);
        public ICommand DeleteLocationCommand => new Command<Location>(selected => List.Locations.Remove(selected));
        public ICommand AddLocationCommand => new Command(async () => await _navigator.PushAsync<LocationListViewModel>());
        public ICommand OpenConfigurationCommand => new Command(async () => await _navigator.PushAsync<ConfigurationViewModel>());

        public ICommand SetWalkModeCommand => new Command(() => TravelMode = TravelMode.Walking);
        public ICommand SetBikeModeCommand => new Command(() => TravelMode = TravelMode.Bicycling);
        public ICommand SetCarModeCommand => new Command(() => TravelMode = TravelMode.Driving);

        public ICommand OpenInGoogleMapsCommand => new Command(() =>
        {
            _googleMapsService.OpenInGoogleMaps(MapViewModel.CalculatedRoute.RouteCoordinates.Select(x => $"{x.Latitude}, {x.Longitude}").ToArray());
        });
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MapViewModel.IsRouteVisible = false;
            NotifyOfPropertyChange(() => MapViewModel.IsRouteVisible);
            RouteCalculated = false;
            NotifyOfPropertyChange(() => RouteCalculated);
        }
        private void ReaorderLocations(int[] waypoints)
        {
            var length = waypoints.Length;
            var result = new int[length];

            int index = Array.FindIndex(waypoints, x => x == 0);

            Array.ConstrainedCopy(waypoints, index, result, 0, length - index);
            Array.ConstrainedCopy(waypoints, 0, result, length - index, index);

            List.Locations.CollectionChanged -= Locations_CollectionChanged;
            List.Reorder(result);
            List.Locations.CollectionChanged += Locations_CollectionChanged;
        }
    }
}
