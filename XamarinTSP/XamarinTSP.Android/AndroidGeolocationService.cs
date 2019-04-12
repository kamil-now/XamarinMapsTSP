using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

[assembly: Dependency(typeof(XamarinTSP.Droid.AndroidGeolocationService))]
namespace XamarinTSP.Droid
{
    public class AndroidGeolocationService : IGeolocationService
    {
        private GeolocationConfiguration _configuration;
        private IGeolocator _locator;
        private Android.Locations.Geocoder _geocoder;
        public AndroidGeolocationService()
        {
            _configuration = new GeolocationConfiguration();

            _locator = CrossGeolocator.Current;
            _locator.DesiredAccuracy = _configuration.LocationAccuracy;
            _geocoder = new Android.Locations.Geocoder(Android.App.Application.Context);
        }
        public async Task<IEnumerable<Xamarin.Forms.Maps.Position>> GetLocationCoordinates(string locationName)
        {
            var positions = await _geocoder.GetFromLocationNameAsync(locationName, _configuration.MaxResults);

            return positions?.Select(p => new Xamarin.Forms.Maps.Position(p.Latitude, p.Longitude));
        }
    }
}