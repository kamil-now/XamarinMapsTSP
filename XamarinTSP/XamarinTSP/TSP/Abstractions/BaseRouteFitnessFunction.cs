using System;
using System.Collections.Generic;

namespace XamarinTSP.TSP.Abstractions
{
    public abstract class BaseRouteFitnessFunction : BaseFitnessFunction, IRouteFitnessFunction
    {
        private int[][] durationData;

        private BaseRouteFitnessFunction(IEnumerable<object> input, int[][] data) : base(input, data)
        {
        }
        public BaseRouteFitnessFunction(IEnumerable<object> input, int[][] distanceData, int[][] durationData) : base(input, distanceData)
        {
            this.durationData = durationData;
        }
        public override void SetFitness<T>(Population<T> population)
        {
            foreach (var element in population.Elements)
            {
                if (element is IRouteElement routeElement)
                {
                    routeElement.DurationValue = CalculateValue(element, durationData);
                    routeElement.DistanceValue = CalculateValue(element, data);
                    routeElement.Fitness = CalculateFitness(element);
                }
                else
                {
                    throw new ArgumentException();
                }

            }

        }
    }
}
