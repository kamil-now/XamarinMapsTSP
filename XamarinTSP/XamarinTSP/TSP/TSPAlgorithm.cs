using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;
using XamarinTSP.UI.Models;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class TSPAlgorithm
    {
        private TSPConfiguration _config;
        private bool _run;
        public TSPAlgorithm(TSPConfiguration configuration)
        {
            _config = configuration;

        }
        public void Stop() => _run = false;
        public void Run(ITSPData tspData, Action<Route> displayRoute)
        {
            _run = true;
            var isValid = _config.Validate();
            if (!isValid)
            {
                throw new Exception("INVALID TSP CONFIGURATION");
            }
            Population population = new Population(_config.PopulationSize, tspData.ElementSize);

            tspData.SetStats(population);

            Element currentBest = population.Best.Copy();
            displayRoute(FormatResult(currentBest, tspData));
            while (_run)
            {
                _config.CrossoverAlgorithm.Crossover(population);
                foreach (var item in population.Elements)
                {
                    if (Helper.RandomPercent() < _config.MutationChance)
                    {
                        item.Mutate();
                    }
                }
                tspData.SetStats(population);

                population = _config.SelectionAlgorithm.Select(population, _config.PopulationSize);

                if (Helper.RandomPercent() < _config.ElitismChance)
                {
                    int elitism = (int)(_config.ElitismFactor * population.Size);
                    for (int j = 0; j < elitism; j++)
                    {
                        population.Add(population.Best.Copy());
                    }
                }

                if (_config.MutationBasedOnDiversity)
                {
                    var diversity = population.Diversity;
                    _config.MutationChance = 1 - diversity - 0.2;
                }

                if (population.Best.DistanceValue < currentBest.DistanceValue)
                {
                    currentBest = population.Best.Copy();
                    displayRoute(FormatResult(currentBest, tspData));
                }
            }
        }

        private Route FormatResult(Element element, ITSPData tspData)
        {
            var waypoints = element.Waypoints;
            var length = waypoints.Length;
            var result = new int[length];

            int index = Array.FindIndex(element.Waypoints, x => x == 0);

            Array.ConstrainedCopy(waypoints, index, result, 0, length - index);
            Array.ConstrainedCopy(waypoints, 0, result, length - index, index);

            var list = new List<Location>();
            for (int i = 0; i < result.Length; i++)
            {
                list.Add(tspData.Input.ElementAt(result[i]));
            }
            if (_config.ReturnToOrigin)
                list.Add(tspData.Input.ElementAt(0));

            return new Route()
            {
                RouteCoordinates = list.Select(x => x.Position).ToList(),
                Distance = Distance.FromMeters(Math.Round(element.DistanceValue)),
                Time = TimeSpan.FromSeconds(element.TimeValue)

            };
        }
    }
}