using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
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
        private bool? _permissionGranted = null;
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

        public async Task<IEnumerable<Location>> GetLocationListAsync(string locationName)
        {
            var addressList = await _geocoder.GetFromLocationNameAsync(locationName, _configuration.MaxResults).ConfigureAwait(false);
            return GetLocations(addressList);
        }
        public async Task<IEnumerable<Location>> GetLocationListAsync(Xamarin.Forms.Maps.Position position)
        {
            var addressList = await _geocoder.GetFromLocationAsync(position.Latitude, position.Longitude, _configuration.MaxResults).ConfigureAwait(false);
            return GetLocations(addressList);
        }
        public IEnumerable<Location> GetLocationList(Xamarin.Forms.Maps.Position position)
        {
            var addressList = _geocoder.GetFromLocationAsync(position.Latitude, position.Longitude, _configuration.MaxResults).Result;
            return GetLocations(addressList);
        }
        private IEnumerable<Location> GetLocations(IList<Android.Locations.Address> addressList)
        {
            return addressList.Select(x =>
            {
                var location = new Location
                {
                    PostalCode = x.PostalCode,
                    City = x.FeatureName,
                    Street = $"{x.Thoroughfare} {x.SubThoroughfare}",
                    Country = x.CountryName,
                    AdminArea = $"{x.AdminArea} { x.SubAdminArea}",
                    Position = new Xamarin.Forms.Maps.Position(x.Latitude, x.Longitude)
                };
                return location;
            });
        }
    }
}