using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinTSP.Common.Abstractions
{
    public interface IGeolocationService
    {
        Task<IEnumerable<Xamarin.Forms.Maps.Position>> GetLocationCoordinates(string locationName);

        Task<IEnumerable<Address>> GetAddressListAsync(string locationName);
        Task<IEnumerable<Address>> GetAddressListAsync(Xamarin.Forms.Maps.Position position);
        IEnumerable<Address> GetAddressList(Xamarin.Forms.Maps.Position position);
        IEnumerable<Address> GetAddressList(string locationName);
    }
}
