using System;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class TSPAlgorithm
    {
        TSPConfiguration _config;

        public TSPAlgorithm(TSPConfiguration configuration)
        {
            _config = configuration;

        }
        public (double time, double distance, int[] waypoints) Run(ITSPData tspData, int populationsCount)
        {
            var isValid = _config.Validate();
            if (!isValid)
            {
                throw new Exception("INVALID TSP CONFIGURATION");
            }
            Population population = new Population(_config.PopulationSize, tspData.ElementSize);

            tspData.SetStats(population);

            Element currentBest = population.Best.Copy();
            for (int populationNumber = 0; populationNumber < populationsCount; populationNumber++)
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
                }
            }

            return FormatResult(currentBest);
        }
        private (double time, double distance, int[] waypoints) FormatResult(Element element)
        {
            var waypoints = element.Waypoints;
            var length = waypoints.Length;
            var result = new int[length];

            int index = Array.FindIndex(element.Waypoints, x => x == 0);

            Array.ConstrainedCopy(waypoints, index, result, 0, length - index);
            Array.ConstrainedCopy(waypoints, 0, result, length - index, index);

            return (element.TimeValue, element.DistanceValue, result);
        }
    }
}