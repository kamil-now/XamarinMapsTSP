using System;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class TSPAlgorithm : ITSPAlgorithm
    {

        public ITSPConfiguration Configuration { get; }
        private bool _run;
        public TSPAlgorithm(ITSPConfiguration configuration)
        {
            Configuration = configuration;

        }
        public void Stop() => _run = false;
        public void Run(ITSPData tspData, Action<Element, ITSPData> renderRoute)
        {
            _run = true;
            Population population = new Population(Configuration.PopulationSize, tspData.ElementSize);

            tspData.SetStats(population);

            Element currentBest = population.Best.Copy();
            renderRoute(currentBest, tspData);
            while (_run)
            {
                Configuration.CrossoverAlgorithm.Crossover(population, Configuration.CrossoverChance);
                foreach (var item in population.Elements)
                {
                    if (Random.RandomPercent() < Configuration.MutationChance)
                    {
                        item.Mutate();
                    }
                }
                tspData.SetStats(population);

                population = Configuration.SelectionAlgorithm.Select(population, Configuration.PopulationSize);

                if (Random.RandomPercent() < Configuration.ElitismChance)
                {
                    int elitism = (int)(Configuration.ElitismFactor * population.Size);
                    for (int j = 0; j < elitism; j++)
                    {
                        population.Add(population.Best.Copy());
                    }
                }

                if (Configuration.MutationBasedOnDiversity)
                {
                    var diversity = population.Diversity;
                    Configuration.MutationChance = 1 - diversity - 0.2;
                }

                if (population.Best.DistanceValue < currentBest.DistanceValue)
                {
                    currentBest = population.Best.Copy();
                    renderRoute(currentBest, tspData);
                }
            }
        }
    }
}