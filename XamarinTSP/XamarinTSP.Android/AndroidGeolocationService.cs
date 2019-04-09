using Android.Locations;
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
        private Geocoder _geocoder;
        public AndroidGeolocationService()
        {
            _configuration = new GeolocationConfiguration();

            _locator = CrossGeolocator.Current;
            _locator.DesiredAccuracy = _configuration.LocationAccuracy;
            _geocoder = new Geocoder(Android.App.Application.Context);
        }
        public async Task<IEnumerable<Position>> SearchLocation(string locationName)
        {
            var addresses = await _geocoder.GetFromLocationNameAsync(locationName, _configuration.MaxResults);

            return addresses.Select(p => new Position(p.Latitude, p.Longitude));
        }
    }
}