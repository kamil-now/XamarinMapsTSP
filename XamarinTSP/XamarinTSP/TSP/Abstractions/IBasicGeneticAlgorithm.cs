using System;
using System.Collections.Generic;

namespace XamarinTSP.TSP.Abstractions
{
    public interface IBasicGeneticAlgorithm
    {
        IBasicGeneticAlgorithmConfiguration Configuration { get; }
        void Run<TElement, TFitnessFunction>(IEnumerable<object> input, int[][] data, Action<TElement, IFitnessFunction> renderRoute)
            where TElement : class, IElement where TFitnessFunction : IFitnessFunction;
        void Stop();
    }
}
