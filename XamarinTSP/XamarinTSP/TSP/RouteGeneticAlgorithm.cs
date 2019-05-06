using System;
using System.Collections.Generic;
using XamarinTSP.GoogleMapsApi.Abstractions;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class RouteGeneticAlgorithm : BasicGeneticAlgorithm, IRouteGeneticAlgorithm
    {
        public RouteGeneticAlgorithm(IBasicGeneticAlgorithmConfiguration configuration) : base(configuration)
        {
        }

        public void RUN<T>(IDistanceMatrixData distanceMatrixData, IEnumerable<object> input, Action<T, IFitnessFunction> renderRouteAction) where T : IElement
        {
            var fitnessFunction = new RouteFitnessFunction(input, distanceMatrixData.DistanceMatrix, distanceMatrixData.DurationMatrix);
            base.RUN(fitnessFunction, renderRouteAction);
        }
    }
}
