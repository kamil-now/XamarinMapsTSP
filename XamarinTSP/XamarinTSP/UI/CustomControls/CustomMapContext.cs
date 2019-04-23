using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;
using XamarinTSP.UI.Models;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.CustomControls
{
    public class CustomMapContext : PropertyChangedBase
    {
        private IGeolocationService _geolocation;
        private LocationList _list;
        public LocationList List => _list;
        public CustomMap CustomMap { get; set; }
        public Route CalculatedRoute { get; set; }

        public CustomMapContext(LocationList list, IGeolocationService geolocation)
        {
            _list = list;
            _geolocation = geolocation;


            CalculatedRoute = new Route();
            _list.Locations.CollectionChanged += ListChanged;
        }
        public void InitLocationPins()
        {
            if (_list?.Locations?.Count != 0 && CustomMap != null)
            {
                _list.Locations.ForEach(location => SetNewLocationPin(location));
            }
        }
        public void DisplayRoute(List<Position> route) => CustomMap.RouteCoordinates = route;
        public async Task MoveToUserRegion() => await MoveToLocation(RegionInfo.CurrentRegion.DisplayName);
        public async Task MoveToLocation(string locationName)
        {
            var positions = await _geolocation.GetLocationCoordinates(locationName);
            if (positions != null)
            {
                var pos = new Position(positions.First().Latitude, positions.First().Longitude);
                CustomMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromKilometers(1)));
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
        public void FocusOnPins() => CustomMap.MoveToRegion(MapSpanGenerator.Generate(CustomMap.Pins.Select(x => x.Position)));
        public void AddPin(Location location)
        {
            var pin = new Pin
            {
                AutomationId = location.Id.ToString(),
                Position = location.Position,
                Label = location.MainDisplayString
            };
            CustomMap.Pins.Add(pin);
            FocusOnPins();
        }
        public void UpdatePin(Location location)
        {
            var pin = CustomMap.Pins.FirstOrDefault(x => x.AutomationId == location.Id.ToString());
            if (pin == null)
                return;
            pin.Position = location.Position;
            pin.Label = location.MainDisplayString;
            FocusOnPins();
        }
        public void RemovePin(Location location)
        {
            var pin = CustomMap.Pins.FirstOrDefault(x => x.AutomationId == location.Id.ToString());
            if (pin == null)
                return;
            CustomMap.Pins.Remove(pin);
            FocusOnPins();
        }
        private void SetNewLocationPin(Location location)
        {
            AddPin(location);
            location.OnDispose += (s, e) =>
            {
                RemovePin(location);
            };
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
