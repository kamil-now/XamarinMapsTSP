using System;
using System.Collections.Generic;
using System.Linq;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class TournamentSelection : ITournamentSelectionAlgorithm
    {
        public string Name => "Tournament";

        public int TournamentSize { get; set; }

        public Population<T> Select<T>(Population<T> population, int count) where T : IElement
        {
            if (TournamentSize <= 0)
                throw new ArgumentException("Tournament size is incorrect");

            var selected = new List<IElement>();
            for (int i = 0; i < count; i++)
            {
                var best = RunTournament(population);
                selected.Add(best.Copy());
            }
            return new Population<T>(selected);
        }

        private IElement RunTournament<T>(Population<T> population) where T : IElement
        {
            int size = population.Size;
            double bestFitness = double.NegativeInfinity;
            IElement best = null;
            for (int j = 0; j < TournamentSize; j++)
            {
                var randomIndex = Random.RandomValue(size);
                var randomElement = population.Elements.ElementAt(randomIndex);
                if (bestFitness < randomElement.Fitness)
                {
                    bestFitness = randomElement.Fitness;
                    best = randomElement;

                }
            }
            return best;
        }
    }
}
