using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinTSP.Utilities;

namespace XamarinTSP.Abstractions
{
    public interface IGeolocationService
    {
        Task<IEnumerable<Xamarin.Forms.Maps.Position>> GetLocationCoordinates(string locationName);
        Task<IEnumerable<Location>> GetLocationList(string locationName);
    }
}
