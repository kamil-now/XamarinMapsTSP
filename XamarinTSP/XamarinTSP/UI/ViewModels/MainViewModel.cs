using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.GoogleMapsApi;
using XamarinTSP.TSP;
using XamarinTSP.TSP.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Models;

namespace XamarinTSP.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private static volatile object _lck = new object();

        private GoogleMapsService _googleMapsService;
        private ITSPAlgorithm _tspAlgorithm;
        private INavigator _navigator;
        private IGeolocationService _geolocation;

        public bool IsTSPRunning { get; private set; }
        public bool RouteCalculated { get; private set; }

        public MapViewModel MapViewModel { get; private set; }
        public LocationList List { get; private set; }

        public MainViewModel(INavigator navigator,
                             IGeolocationService geolocation,
                             ITSPAlgorithm tspAlgorithm,
                             MapViewModel mapViewModel,
                             LocationList list,
                             GoogleMapsService googleMapsService)
        {
            List = list;
            MapViewModel = mapViewModel;
            _geolocation = geolocation;
            _googleMapsService = googleMapsService;
            _navigator = navigator;
            _tspAlgorithm = tspAlgorithm;
            List.Locations.CollectionChanged += Locations_CollectionChanged;
        }
        

        public ICommand OnAppearingCommand => new Command(async () =>
        {
            //temp
            if (List.Locations.Count == 0)
            {
                List.SetMockData(_geolocation);
            }
            //
            if (List.Locations?.Count == 0)
                await MapViewModel.MoveToUserRegion();
        });

        public ICommand RunTSPCommand => new Command<Button>(async button =>
        {
            //TODO run unchanged with saved route
            if (IsTSPRunning)
            {
                _tspAlgorithm.Stop();
                IsTSPRunning = false;
                NotifyOfPropertyChange(() => IsTSPRunning);
                return;
            }
            IsTSPRunning = true;
            NotifyOfPropertyChange(() => IsTSPRunning);

            await Task.Run(async () =>
            {
                try
                {
                    var data = _googleMapsService.GetDistanceMatrix(List.Locations.Select(x => $"{x.Position.Latitude},{x.Position.Longitude}").ToArray());


                    _tspAlgorithm.Run(
                        new TSPData(
                            List.Locations as IEnumerable<object>,
                            data.DistanceMatrix,
                            data.DurationMatrix),
                        new Action<TSP.Element, ITSPData>(async (element, tspData) =>
                        {
                            await App.InvokeOnMainThreadAsync(() =>
                            {
                                ReaorderLocations(element.Waypoints);

                                var route = new Route()
                                {
                                    RouteCoordinates = List.Locations.Select(x => x.Position).ToList(),
                                    Distance = Distance.FromMeters(Math.Round(element.DistanceValue)),
                                    Time = TimeSpan.FromSeconds(element.TimeValue)

                                };
                                route.RouteCoordinates.Add(List.Locations.ElementAt(0).Position);

                                MapViewModel.CalculatedRoute = route;
                                RouteCalculated = true;


                                NotifyOfPropertyChange(() => RouteCalculated);
                                MapViewModel.DisplayRoute();
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
        public ICommand SelectLocationCommand => new Command<Location>(selected =>
        {
            MapViewModel.MapPosition = selected.Position;
        });
        public ICommand DeleteLocationCommand => new Command<Location>(selected =>
        {
            List.Locations.Remove(selected);
        });
        public ICommand AddLocationCommand => new Command(async () =>
        {
            await _navigator.PushAsync<LocationListViewModel>();
        });
        public ICommand OpenConfigurationCommand => new Command(async () =>
        {
            await _navigator.PushAsync<ConfigurationViewModel>();
        });
        public ICommand SetWalkModeCommand => new Command(() =>
        {
            //TODO
        });
        public ICommand SetBikeModeCommand => new Command(() =>
        {
            //TODO
        });
        public ICommand SetCarModeCommand => new Command(() =>
        {
            //TODO
        });
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
