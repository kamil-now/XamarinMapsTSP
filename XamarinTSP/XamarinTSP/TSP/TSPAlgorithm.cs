using System;
using System.Diagnostics;
using System.Threading.Tasks;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
   public class TSPAlgorithm
    {
        static int tournamentSize = 5;
        static int populationSize = 100;
        static double crossoverChance = 0.6;
        static double mutationChance = 0.7;
        static double elitism = 50;
        static ISelectionAlgorithm selectionAlgorithm = new TournamentSelection(tournamentSize);
        static ICrossoverAlgorithm crossoverAlgorithm = new PMXCrossover(crossoverChance);
        static IDistanceData distanceData;

        public static int[] RUN(int[][] distanceInfo,int populationsCount, bool returnToOrigin)
        {
            //TODO return to origin option
            distanceData = new DistanceData(distanceInfo, returnToOrigin);

            Population population = new Population(populationSize, distanceInfo.Length);

            distanceData.SetStats(population);
            
            Element currentBest = population.Best.Copy();
            for (int populationNumber = 0; populationNumber < populationsCount; populationNumber++)
            {
                crossoverAlgorithm.Crossover(population);
                foreach (var item in population.Elements)
                {
                    if (Helper.RandomPercent() < mutationChance)
                    {
                        item.Mutate();
                    }
                }
                distanceData.SetStats(population);

                population = selectionAlgorithm.Select(population, populationSize);

                if (populationNumber % 1000 == 0)
                {
                    if (elitism > 3)
                        elitism--;
                    else
                        elitism = 10;
                }
                if (populationNumber > 1000)
                {
                    for (int j = 0; j < elitism; j++)
                    {
                        population.Add(population.Best.Copy());
                    }
                }
                var diversity = population.Diversity;
                mutationChance = 1 - diversity - 0.2;

                if (population.Best.Value < currentBest.Value)
                {
                    currentBest = population.Best.Copy();
                }
            }

            return FormatResult(currentBest.Data);
        }
        private static int[] FormatResult(int[] data)
        {
            var length = data.Length;
            var result = new int[length];

            int index = Array.FindIndex(data, x => x == 0);

            Array.ConstrainedCopy(data, index, result, 0, length - index);
            Array.ConstrainedCopy(data, 0, result, index, index);

            return result;
        }
    }
}