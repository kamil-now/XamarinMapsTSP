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

        public void ListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                var location = item as Location;
                location.PropertyChanged += (s,ev) => AddPin(location.Name);
                
            }
            
        }

        public async Task MoveToUserRegion() => await MoveToLocation(RegionInfo.CurrentRegion.DisplayName);
        public async Task MoveToLocation(string locationName)
        {
            _geolocation = DependencyService.Get<IGeolocationService>();
            var positions = await _geolocation.SearchLocation(locationName);
            if (positions != null)
                this.Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(positions.First().Latitude, positions.First().Longitude), _mapDistance));
            NotifyOfPropertyChange(() => Map);
        }
        public void AddPin(Pin pin)
        {
            Map.Pins.Add(pin);
        }
        public void AddPin(string address)
        {
            var pin = new Pin() { Address = address };
            Map.Pins.Add(pin);
        }
    }

}
