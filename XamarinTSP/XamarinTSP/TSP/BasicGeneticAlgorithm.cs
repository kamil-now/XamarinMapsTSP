using System;
using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class BasicGeneticAlgorithm : IBasicGeneticAlgorithm
    {

        public IBasicGeneticAlgorithmConfiguration Configuration { get; }
        private bool _run;
        public BasicGeneticAlgorithm(IBasicGeneticAlgorithmConfiguration configuration)
        {
            Configuration = configuration;

        }
        public void Stop() => _run = false;

        public void Run<TElement, TFitnessFunction>(IEnumerable<object> input, int[][] data, Action<TElement, IFitnessFunction> renderRouteAction) 
            where TElement : IElement where TFitnessFunction : IFitnessFunction
        {
            var fitnessFunction = FitnessFunctionFactory.Create<TFitnessFunction>(input, data);

            RUN(fitnessFunction, renderRouteAction);
           
        }
        protected void RUN<TElement>(IFitnessFunction fitnessFunction, Action<TElement, IFitnessFunction> renderRoute) where TElement : IElement
        {
            _run = true;
            Population<TElement> population = new Population<TElement>(Configuration.PopulationSize, fitnessFunction.ElementSize);

            fitnessFunction.SetFitness(population);

            TElement currentBest = (TElement)population.Best.Copy();
            renderRoute(currentBest, fitnessFunction);
            while (_run)
            {
                Configuration.CrossoverAlgorithm.Crossover(population, Configuration.CrossoverChance);

                var mutationChance = Configuration.MutationChance;
                if (Configuration.MutationBasedOnDiversity)
                {
                    var diversity = population.Diversity;
                    mutationChance = 1 - diversity - Configuration.MutationChance;
                }
                foreach (var item in population.Elements)
                {
                    if (Random.RandomPercent() < mutationChance)
                    {
                        Configuration.MutationAlgorithm.Mutate(item);
                    }
                }
                fitnessFunction.SetFitness(population);

                population = Configuration.SelectionAlgorithm.Select(population, Configuration.PopulationSize);


                int elitism = (int)(Configuration.ElitismFactor * population.Size);
                for (int j = 0; j < elitism; j++)
                {
                    population.Add(population.Best.Copy());
                }

                if (population.Best.Value < currentBest.Value)
                {
                    currentBest = (TElement)population.Best.Copy();
                    renderRoute(currentBest, fitnessFunction);
                }
            }
        }
    }
}