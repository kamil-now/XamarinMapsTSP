using System.Collections.Generic;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public interface ITSPData
    {
        IEnumerable<Location> Input { get; }
        int ElementSize { get; }
        void SetValue(Population population);
        void SetFitness(Population population);
        void SetStats(Population population);
    }
}