using System.Collections.Generic;

namespace XamarinTSP.TSP.Abstractions
{
    public interface IFitnessFunction
    {
        string Name { get; }
        IEnumerable<object> Input { get; }
        int ElementSize { get; }
        void SetFitness<T>(Population<T> population) where T : IElement;
    }
}