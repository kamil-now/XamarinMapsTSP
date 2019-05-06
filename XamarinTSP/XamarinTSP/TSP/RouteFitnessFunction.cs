using System;
using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class RouteFitnessFunction : BaseRouteFitnessFunction, IRouteFitnessFunction
    {
        public override string Name => "Distance + duration";
        
        public RouteFitnessFunction(IEnumerable<object> input, int[][] distanceData, int[][] durationData) : base(input, distanceData, durationData)
        {
        }

        protected override double CalculateFitness(IElement element)
        {
            if (element is IRouteElement routeElement)
            {
                return 1000 / (routeElement.DistanceValue + routeElement.DurationValue);
            }
            throw new ArgumentException();
        }
    }
}
