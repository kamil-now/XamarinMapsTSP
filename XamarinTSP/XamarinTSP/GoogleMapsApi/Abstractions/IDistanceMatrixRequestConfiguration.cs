using System;
using XamarinTSP.GoogleMapsApi.Enums;

namespace XamarinTSP.GoogleMapsApi.Abstractions
{
    public interface IDistanceMatrixRequestConfiguration
    {
        string[] Destinations { get; set; }
        string[] Origins { get; set; }
        string Origin { get; set; }
        TravelMode TravelMode { get; set; }
        DateTime? DepartureTime { get; set; }
        DateTime? ArrivalTime { get; set; }
        TrafficModel TrafficModel { get; set; }
        Restriction Restriction { get; set; }

        IDistanceMatrixRequestConfiguration Copy();
    }
}
