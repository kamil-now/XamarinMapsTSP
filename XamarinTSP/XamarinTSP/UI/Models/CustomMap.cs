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
        private const int EARTH_RADIUS_KM = 6371;
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
        public CustomMap() { }
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
        public async Task ZoomToPins()
        {
            var points = Map.Pins.Select(x => x.Position);
            var center = CalculateCenter(points);
            var radius = Distance.FromMeters(CalculateRadius(center, points));

            this.Map.MoveToRegion(MapSpan.FromCenterAndRadius(center, radius));

        }
        private Position CalculateCenter(IEnumerable<Position> points)
        {
            return new Position();
        }
        private double CalculateRadius(Position center, IEnumerable<Position> points)
        {
            return 0;
        }
        public double MeasureDistance(Position a, Position b)
        {
            var dLat = DegreeToRadian(b.Latitude - a.Latitude);
            var dLon = DegreeToRadian(b.Longitude - a.Longitude);
            var lat1 = DegreeToRadian(a.Latitude);
            var lat2 = DegreeToRadian(b.Latitude);

            var x = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1 - x));
            return EARTH_RADIUS_KM * c;
        }
        private double DegreeToRadian(double degree) => degree * Math.PI / 180;
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
