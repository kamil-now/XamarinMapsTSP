using System.Collections.Generic;
using System.Linq;
using XamarinTSP.TSP.Common.Abstractions;

namespace XamarinTSP.TSP
{
    public class TournamentSelection : ISelectionAlgorithm
    {
        int tournamentSize;
        public TournamentSelection(int tournamentSize)
        {
            this.tournamentSize = tournamentSize;
        }
        public Population Select(Population population, int count)
        {
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
            for (int j = 0; j < tournamentSize; j++)
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
