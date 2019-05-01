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

        public Population Select(Population population, int count)
        {
            if (TournamentSize <= 0)
                throw new ArgumentException("Tournament size is incorrect");
            //TODO test
            var selected = new List<Element>();
            for (int i = 0; i < count; i++)
            {
                var best = RunTournament(population);
                selected.Add(best.Copy());
            }
            return new Population(selected);
        }

        Element RunTournament(Population population)
        {
            //TODO test
            int size = population.Size;
            double bestFitness = double.NegativeInfinity;
            Element best = null;
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
