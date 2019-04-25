using System;
using System.ComponentModel;

namespace XamarinTSP.Utilities
{
    public class DistanceMatrixRequestConfiguration
    {
        [Description("destinations")]
        public string[] Destinations { get; set; }
        [Description("origins")]
        public string[] Origins { get; set; }
        [Description("mode")]
        public TravelMode TravelMode { get; set; }
        [Description("region")]
        public string Region { get; set; } //region ccTLD code
        [Description("units")]
        public UnitSystem UnitSystem { get; set; }
        [Description("departure_time")]
        public DateTime? DepartureTime { get; set; } //max 100 elements per request when mode = driving
        [Description("arrival_time")]
        public DateTime? ArrivalTime { get; set; }
        [Description("traffic_model")]
        public TrafficModel TrafficModel { get; set; }
        [Description("avoid")]
        public Restriction Restriction { get; set; }
        public DistanceMatrixRequestConfiguration(string[] locations)
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
    public enum TrafficModel
    {
        Undefined,
        [Description("best_guess")]
        BestGuess,
        [Description("pessimistic")]
        Pessimistic,
        [Description("optimistic")]
        Optimistic
    }
    public enum TravelMode
    {
        Undefined,
        [Description("driving")]
        Driving,
        [Description("walking")]
        Walking,
        [Description("bicycling")]
        Bicycling
    }
    public enum Restriction
    {
        Undefined,
        [Description("tolls")]
        AvoidTolls,
        [Description("highways")]
        AvoidHighways,
        [Description("ferries")]
        AvoidFerries,
        [Description("indoor")]
        AvoidIndoor,
    }
    public enum UnitSystem
    {
        Undefined,
        [Description("metric")]
        Metric,
        [Description("imperial")]
        Imperial
    }
}
