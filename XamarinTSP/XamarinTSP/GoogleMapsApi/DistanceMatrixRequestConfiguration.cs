using System;
using XamarinTSP.GoogleMapsApi.Abstractions;
using XamarinTSP.GoogleMapsApi.Enums;

namespace XamarinTSP.GoogleMapsApi
{
    internal class DistanceMatrixRequestConfiguration : IDistanceMatrixRequestConfiguration
    {
        public string[] Destinations { get; set; }
        public string[] Origins { get; set; }
        public TravelMode TravelMode { get; set; }
        public string Region { get; set; } //region ccTLD code
        public UnitSystem UnitSystem { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public TrafficModel TrafficModel { get; set; }
        public Restriction Restriction { get; set; }

        public DistanceMatrixRequestConfiguration()
        {
            TravelMode = TravelMode.Driving;
            UnitSystem = UnitSystem.Metric;
            TrafficModel = TrafficModel.Optimistic;
        }
        public DistanceMatrixRequestConfiguration(string[] locations) : this()
        {
            Destinations = locations;
            Origins = locations;
        }
        public DistanceMatrixRequestConfiguration(string origin, string[] locations, DistanceMatrixRequestConfiguration config)
        {
            Origins = new[] { origin };
            Destinations = locations;

            TravelMode = config.TravelMode;
            Region = config.Region;
            UnitSystem = config.UnitSystem;
            DepartureTime = config.DepartureTime;
            ArrivalTime = config.ArrivalTime;
            TrafficModel = config.TrafficModel;
            Restriction = config.Restriction;
        }
    }
}
