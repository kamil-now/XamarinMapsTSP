using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class MapViewModel : PropertyChangedBase
    {
        public Map Map { get; set; }
        private IGeolocationService _geolocation;
        private Distance _mapDistance;

        public MapViewModel()
        {
            _geolocation = DependencyService.Get<IGeolocationService>();
            _mapDistance = Distance.FromMiles(1000);
            Map = new Map(MapSpan.FromCenterAndRadius(new Position(0, 0), _mapDistance))
            {
                MapType = MapType.Street,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };
        }

        public void ListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args?.NewItems == null)
                return;
            foreach (var item in args?.NewItems)
            {
                if (item is Location location)
                {
                    location.OnDispose += (s, e) => RemovePin(location.Pin);
                    location.OnEdit += async (s, e) =>
                    {
                        Position? selectedPosition = null;

                        if (!string.IsNullOrEmpty(location.Name))
                        {
                            var positions = await _geolocation.GetLocationCoordinates(location.Name);
                            //TODO select from list
                            selectedPosition = positions.FirstOrDefault();
                        }

                        if (selectedPosition != null)
                        {
                            location.SetPinPosition((Position)selectedPosition);

                            AddOrUpdatePin(location.Pin);
                        }
                        else
                        {
                            RemovePin(location.Pin);
                        }
                    };
                    
                }
            }
        }

        public async Task MoveToUserRegion() => await MoveToLocation(RegionInfo.CurrentRegion.DisplayName);
        public async Task MoveToLocation(string locationName)
        {
            _geolocation = DependencyService.Get<IGeolocationService>();
            var positions = await _geolocation.GetLocationCoordinates(locationName);
            if (positions != null)
            {
                var pos = new Position(positions.First().Latitude, positions.First().Longitude);
                this.Map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, _mapDistance));
            }
            NotifyOfPropertyChange(() => Map);
        }
        public void AddOrUpdatePin(Pin pin)
        {
            if (!Map.Pins.Contains(pin))
            {
                Map.Pins.Add(pin);
            }
            NotifyOfPropertyChange(() => Map);
        }
        public void RemovePin(Pin pin)
        {
            if (Map.Pins.Contains(pin))
            {
                Map.Pins.Remove(pin);
            }
            NotifyOfPropertyChange(() => Map);
        }
    }

}
