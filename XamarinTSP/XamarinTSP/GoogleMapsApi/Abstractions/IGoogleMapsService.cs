using System.Collections.Generic;
using XamarinTSP.GoogleMapsApi.Enums;

namespace XamarinTSP.GoogleMapsApi.Abstractions
{
    public interface IGoogleMapsService
    {
        IDistanceMatrixData GetDistanceMatrix(IEnumerable<string> locations, TravelMode travelMode);
        void OpenInGoogleMaps(string[] waypoints);
    }
}
