using System;

namespace XamarinTSP.TSP.Common.Abstractions
{
    public interface ITSPAlgorithm
    {
        ITSPConfiguration Configuration { get; }
        void Run(ITSPData tspData, Action<Element, ITSPData> renderRoute);
        void Stop();
    }
}
