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
        public int[] Run(IDistanceData distanceData, int populationsCount)
        {
            var isValid = _config.Validate();
            if (!isValid)
            {
                throw new Exception("INVALID TSP CONFIGURATION");
            }
            Population population = new Population(_config.PopulationSize, distanceData.ElementSize);

            distanceData.SetStats(population);

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
                distanceData.SetStats(population);

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

                if (population.Best.Value < currentBest.Value)
                {
                    currentBest = population.Best.Copy();
                }
            }

            return FormatResult(currentBest.Data);
        }
        private int[] FormatResult(int[] data)
        {
            var length = data.Length;
            var result = new int[length];

            int index = Array.FindIndex(data, x => x == 0);

            Array.ConstrainedCopy(data, index, result, 0, length - index);
            Array.ConstrainedCopy(data, 0, result, length - index, index);

            return result;
        }
    }
}