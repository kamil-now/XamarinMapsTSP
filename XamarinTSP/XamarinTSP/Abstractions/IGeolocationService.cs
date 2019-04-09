
using Plugin.Geolocator.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinTSP.Abstractions
{
    public interface IGeolocationService
    {
        Task<IEnumerable<Position>> SearchLocation(string locationName);
    }
}
