using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarinTSP.Abstractions
{
    public interface IGeolocationService
    {
        Task<IEnumerable<Xamarin.Forms.Maps.Position>> GetLocationCoordinates(string locationName);
    }
}
