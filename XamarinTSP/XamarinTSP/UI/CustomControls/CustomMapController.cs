using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using XamarinTSP.Abstractions;
using XamarinTSP.UI.Models;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.CustomControls
{
    public class CustomMapController : PropertyChangedBase
    {
        private IGeolocationService _geolocation;
        public CustomMap CustomMap { get; set; }
        public Route CalculatedRoute { get; set; }
        public LocationList List { get; set; }
        public CustomMapController(IGeolocationService geolocation, LocationList list)
        {
            _geolocation = geolocation;
            List = list;
            CalculatedRoute = new Route();
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
    }
}
