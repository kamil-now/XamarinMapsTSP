using System;
using System.Collections.Generic;
using System.Linq;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class RouteElement : IRouteElement
    {
        public double Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public double DistanceValue { get; set; } = -1;
        public double DurationValue { get; set; } = -1;
        public double Fitness { get; set; } = -1;

        public int[] Data { get; }
        public RouteElement(int size) : this(Random.GetRandomData(size)) { }
        public RouteElement(IEnumerable<int> data)
        {
            Data = data.ToArray();
        }
        public RouteElement(int[] data, double distanceValue, double durationValue, double fitness)
        {
            Data = data;
            DistanceValue = distanceValue;
            DurationValue = durationValue;
            Fitness = fitness;
        }
        public IElement Copy() => new RouteElement(Data, DistanceValue, DurationValue, Fitness);
    }
}
