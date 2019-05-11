using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Common;
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
        private IGoogleMapsService _googleMapsService;
        private IRouteGeneticAlgorithm _tspAlgorithm;
        private INavigator _navigator;
        private IGeolocationService _geolocation;
        private TravelMode _travelMode;
        private bool _isAlgorithmRunning;
        private bool _routeCalculated;
        private bool _retrievingDistanceMatrix;

        public bool IsAlgorithmRunning
        {
            get => _isAlgorithmRunning || _retrievingDistanceMatrix;
            private set
            {
                _isAlgorithmRunning = value;
                NotifyOfPropertyChange();
            }
        }
        public bool RouteCalculated
        {
            get => _routeCalculated;
            private set
            {
                _routeCalculated = value;
                NotifyOfPropertyChange();
            }
        }
        public bool RetrievingDistanceMatrix
        {
            get => _retrievingDistanceMatrix;
            private set
            {
                _retrievingDistanceMatrix = value;
                NotifyOfPropertyChange(() => IsAlgorithmRunning);
                NotifyOfPropertyChange();
            }
        }

        public bool LoadingSampleData { get; private set; }

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
             if (List.Locations?.Count == 0)
            {
                MapViewModel.ZoomDistance = 1000;
                await MapViewModel.MoveToUserRegion();
            }
        });

        public ICommand RunTSPCommand => new Command<Button>(async button =>
        {
            var renderActionQueue = new DelegateInvocationQueue() { MillisecondsTimeout = 5000 };

            if (IsAlgorithmRunning || RetrievingDistanceMatrix)
            {
                _tspAlgorithm.STOP();
                renderActionQueue.ClearQueue();

                IsAlgorithmRunning = false;
                RetrievingDistanceMatrix = false;
                return;
            }

            RetrievingDistanceMatrix = true;
            await Task.Run(async () =>
            {
                try
                {
                    var data = _googleMapsService.GetDistanceMatrix(List.Locations.Select(x => $"{x.Position.Latitude},{x.Position.Longitude}").ToArray(), TravelMode);
                    await App.InvokeOnMainThreadAsync(() =>
                    {
                        RetrievingDistanceMatrix = false;
                        IsAlgorithmRunning = true;
                    });
                    var renderAction = new Action<IRouteElement, IFitnessFunction>(async (element, fitnessFunction) =>
                     {
                         await App.InvokeOnMainThreadAsync(() =>
                         {
                             ReaorderLocations(element.Data);
                             NotifyOfPropertyChange(() => List.Locations);

                             var route = new Route()
                             {
                                 RouteCoordinates = List.Locations.Select(x => x.Position).ToList(),
                                 Distance = Distance.FromMeters(Math.Round(element.Value)),
                                 Time = TimeSpan.FromSeconds(element.DurationValue)

                             };
                             route.RouteCoordinates.Add(List.Locations.ElementAt(0).Position);

                             MapViewModel.CalculatedRoute = route;
                             RouteCalculated = true;


                             MapViewModel.DisplayRoute();

                         });
                     });

                    _tspAlgorithm.RUN(
                        data,
                        List.Locations as IEnumerable<object>,
                        new Action<IRouteElement, IFitnessFunction>(
                            async (elementParam, fitnessFunctionParam)
                                => await renderActionQueue.InvokeNext(renderAction, elementParam, fitnessFunctionParam)
                        )
                    );

                }
                catch (Exception ex)
                {
                    _tspAlgorithm.STOP();
                    renderActionQueue.ClearQueue();
                    await App.InvokeOnMainThreadAsync(async () =>
                    {
                        IsAlgorithmRunning = false;
                        RetrievingDistanceMatrix = false;
                        await Application.Current.MainPage.DisplayAlert("TSP ERROR", ex.Message, "OK");

                    });
                    return;
                }
            }).ConfigureAwait(false);
        });
        public ICommand SelectLocationCommand => new Command<Location>(selected => MapViewModel.MapPosition = selected.Position);
        public ICommand DeleteLocationCommand => new Command<Location>(selected => List.Locations.Remove(selected));

        public ICommand OpenConfigurationCommand => new Command(async () => await _navigator.PushAsync<ConfigurationViewModel>());
        public ICommand AddLocationCommand => new Command(async () =>
        {
            if (List.Locations.Count >= _googleMapsService.MAX_REQUEST_DESTINATIONS_COUNT)
            {
                await Application.Current.MainPage.DisplayAlert("REACHED LIMIT",
                    $"Distance matrix API destinations equals: {_googleMapsService.MAX_REQUEST_DESTINATIONS_COUNT}", "OK");
            }
            await _navigator.PushAsync<LocationListViewModel>();
        });

        public ICommand SetWalkModeCommand => new Command(() => TravelMode = TravelMode.Walking);
        public ICommand SetBikeModeCommand => new Command(() => TravelMode = TravelMode.Bicycling);
        public ICommand SetCarModeCommand => new Command(() => TravelMode = TravelMode.Driving);
        public ICommand SetSampleDataCommand => new Command(async () =>
        {
            LoadingSampleData = true;

            NotifyOfPropertyChange(() => LoadingSampleData);
            await Task.Run(async () =>
            {
                List.SetMockData(_geolocation);
                await App.InvokeOnMainThreadAsync(() =>
                {
                    foreach (var item in List.Locations)
                    {
                        Debug.WriteLine($"{item.Position.Latitude} | {item.Position.Longitude}");
                    }
                    LoadingSampleData = false;
                    NotifyOfPropertyChange(() => LoadingSampleData);

                });
            });
        });

        public ICommand OpenInGoogleMapsCommand => new Command(() =>
        {
            _googleMapsService.OpenInGoogleMaps(List.Locations.Concat(new[] { List.Locations.First() }).Select(x => x.IsSampleData ? x.Name : $"{x.Position.Latitude}+{x.Position.Longitude}").ToArray());
        });
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MapViewModel.IsRouteVisible = false;
            MapViewModel.NotifyOfPropertyChange(() => MapViewModel.IsRouteVisible);
            RouteCalculated = false;
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
