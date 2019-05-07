using System;
using XamarinTSP.GoogleMapsApi.Enums;

namespace XamarinTSP.GoogleMapsApi.Abstractions
{
    public interface IDistanceMatrixRequestConfiguration
    {
        string[] Destinations { get; set; }
        string[] Origins { get; set; }
        TravelMode TravelMode { get; set; }
        UnitSystem UnitSystem { get; set; }
        DateTime? DepartureTime { get; set; }
        DateTime? ArrivalTime { get; set; }
        TrafficModel TrafficModel { get; set; }
        Restriction Restriction { get; set; }
    }
}
