using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTSP.Common;
using XamarinTSP.Common.Abstractions;

[assembly: Dependency(typeof(XamarinTSP.Droid.AndroidGeolocationService))]
namespace XamarinTSP.Droid
{
    public class AndroidGeolocationService : IGeolocationService
    {
        public int MaxResults { get; set; } = 10;
        private Android.Locations.Geocoder _geocoder;

        public AndroidGeolocationService()
        {
            _geocoder = new Android.Locations.Geocoder(Android.App.Application.Context);
        }
        public async Task<IEnumerable<Xamarin.Forms.Maps.Position>> GetLocationCoordinates(string locationName)
        {
            var positions = await _geocoder.GetFromLocationNameAsync(locationName, MaxResults);
            return positions?.Select(p => new Xamarin.Forms.Maps.Position(p.Latitude, p.Longitude));
        }

        public async Task<IEnumerable<Address>> GetAddressListAsync(string locationName)
        {
            var addressList = await _geocoder.GetFromLocationNameAsync(locationName, MaxResults).ConfigureAwait(false);
            return Map(addressList);
        }
        public async Task<IEnumerable<Address>> GetAddressListAsync(Xamarin.Forms.Maps.Position position)
        {
            var addressList = await _geocoder.GetFromLocationAsync(position.Latitude, position.Longitude, MaxResults).ConfigureAwait(false);
            return Map(addressList);
        }
        public IEnumerable<Address> GetAddressList(Xamarin.Forms.Maps.Position position)
        {
            var addressList = _geocoder.GetFromLocationAsync(position.Latitude, position.Longitude, MaxResults).Result;
            return Map(addressList);
        }
        private IEnumerable<Address> Map(IList<Android.Locations.Address> addressList)
        {
            return addressList.Select(x =>
                new Address()
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    CountryCode = x.CountryCode,
                    CountryName = x.CountryName,
                    FeatureName = x.FeatureName,
                    PostalCode = x.PostalCode,
                    SubLocality = x.SubLocality,
                    Thoroughfare = x.Thoroughfare,
                    SubThoroughfare = x.SubThoroughfare,
                    Locality = x.Locality,
                    AdminArea = x.AdminArea,
                    SubAdminArea = x.SubAdminArea
                }
            );
        }
    }
}