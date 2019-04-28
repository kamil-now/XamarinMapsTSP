using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.CustomControls;
using XamarinTSP.UI.Models;

namespace XamarinTSP.UI.ViewModels
{
    public class CustomMapViewModel : PropertyChangedBase
    {
        private IGeolocationService _geolocation;
        public CustomMap CustomMap { get; set; }
        public Route CalculatedRoute { get; set; }
        public LocationList List { get; set; }
        public CustomMapViewModel(IGeolocationService geolocation, LocationList list)
        {
            _geolocation = geolocation;
            List = list;
            CalculatedRoute = new Route();
        }
        public void DisplayRoute()
        {
            CustomMap.RouteCoordinates = CalculatedRoute.RouteCoordinates;
            NotifyOfPropertyChange(() => CalculatedRoute.Time);
            NotifyOfPropertyChange(() => CalculatedRoute.Distance);
        }
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
    }
}
