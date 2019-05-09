using System.Collections.Generic;
using XamarinTSP.GoogleMapsApi.Enums;

namespace XamarinTSP.GoogleMapsApi.Abstractions
{
    public interface IGoogleMapsService
    {
        int MAX_REQUEST_DESTINATIONS_COUNT { get; }
        IDistanceMatrixData GetDistanceMatrix(IEnumerable<string> locations, TravelMode travelMode);
        void OpenInGoogleMaps(string[] waypoints);
    }
}
