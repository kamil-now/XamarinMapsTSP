using System;
using System.Collections.Generic;
using XamarinTSP.GoogleMapsApi.Abstractions;

namespace XamarinTSP.TSP.Abstractions
{
    public interface IRouteGeneticAlgorithm : IBasicGeneticAlgorithm
    {
        void RUN<T>(IDistanceMatrixData distanceMatrixData, IEnumerable<object> input, Action<T, IFitnessFunction> renderRoute) where T : class, IElement;
    }
}
