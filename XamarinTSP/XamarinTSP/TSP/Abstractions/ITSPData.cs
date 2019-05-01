using System.Collections.Generic;

namespace XamarinTSP.TSP.Abstractions
{
    public interface ITSPData
    {
        IEnumerable<object> Input { get; }
        int ElementSize { get; }
        void SetValue(Population population);
        void SetFitness(Population population);
        void SetStats(Population population);
    }
}