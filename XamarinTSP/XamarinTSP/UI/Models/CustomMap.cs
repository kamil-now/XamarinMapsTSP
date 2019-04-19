using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class CustomMap : PropertyChangedBase
    {

        private IGeolocationService _geolocation;
        private Distance _mapDistance;
        public Map Map { get; set; }
        private LocationList _list;
        public CustomMap(LocationList list, IGeolocationService geolocation)
        {
            _list = list;
            _geolocation = geolocation;
            _mapDistance = Distance.FromMiles(1000);
            Map = new Map(MapSpan.FromCenterAndRadius(new Position(0, 0), _mapDistance))
            {
                MapType = MapType.Street,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            _list.Locations.CollectionChanged += ListChanged;
            if (_list?.Locations?.Count != 0)
            {
                _list.Locations.ForEach(location => SetNewLocationPin(location));
            }
        }
        public void ListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args?.NewItems == null)
                return;
            foreach (var item in args?.NewItems)
            {
                if (item is Location location)
                {
                    SetNewLocationPin(location);
                }
            }
        }
        public void FocusOnPins() => Map.MoveToRegion(MapSpanGenerator.Generate(Map.Pins.Select(x => x.Position)));
        public async Task MoveToUserRegion() => await MoveToLocation(RegionInfo.CurrentRegion.DisplayName);
        public async Task MoveToLocation(string locationName)
        {
            var positions = await _geolocation.GetLocationCoordinates(locationName);
            if (positions != null)
            {
                var pos = new Position(positions.First().Latitude, positions.First().Longitude);
                this.Map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, _mapDistance));
            }
            NotifyOfPropertyChange(() => Map);
        }
        public void AddPin(Location location)
        {
            var pin = new Pin
            {
                AutomationId = location.Id.ToString(),
                Position = location.Position,
                Label = location.MainDisplayString
            };
            Map.Pins.Add(pin);
            NotifyOfPropertyChange(() => Map);
        }
        public void UpdatePin(Location location)
        {
            var pin = Map.Pins.FirstOrDefault(x => x.AutomationId == location.Id.ToString());
            if (pin == null)
                return;
            pin.Position = location.Position;
            pin.Label = location.MainDisplayString;
            NotifyOfPropertyChange(() => Map);
        }
        public void RemovePin(Location location)
        {
            var pin = Map.Pins.FirstOrDefault(x => x.AutomationId == location.Id.ToString());
            if (pin == null)
                return;
            Map.Pins.Remove(pin);
            NotifyOfPropertyChange(() => Map);
        }
        private void SetNewLocationPin(Location location)
        {
            AddPin(location);
            location.OnDispose += (s, e) => RemovePin(location);
            location.PositionChanged += (s, e) =>
            {
                if (location?.Position == null)
                {
                    RemovePin(location);
                }
                else
                {
                    UpdatePin(location);
                }
            };
        }
    }

}
