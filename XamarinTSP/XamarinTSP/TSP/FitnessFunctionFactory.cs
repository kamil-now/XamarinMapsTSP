using System;
using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class FitnessFunctionFactory
    {
        public static IFitnessFunction Create<T>(IEnumerable<object> input, int[][] data) where T : IFitnessFunction
        {
            if (typeof(T) == typeof(IDistanceFitnessFunction))
            {
                return new DistanceFitnessFunction(input, data);
            }
            else if (typeof(T) == typeof(IDurationFitnessFunction))
            {
                return new DurationFitnessFunction(input, data);
            }
            throw new ArgumentException();
        }
    }
}
