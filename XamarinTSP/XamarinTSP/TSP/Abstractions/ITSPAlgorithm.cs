using System;

namespace XamarinTSP.TSP.Abstractions
{
    public interface ITSPAlgorithm
    {
        ITSPConfiguration Configuration { get; }
        void Run(ITSPData tspData, Action<Element, ITSPData> renderRoute);
        void Stop();
    }
}
