using System.Linq;
using XamarinTSP.TSP.Common.Abstractions;

namespace XamarinTSP.TSP
{
    public class PMXCrossover : ICrossoverAlgorithm
    {
        double crossoverChance;
        public PMXCrossover(double crossoverChance)
        {
            this.crossoverChance = crossoverChance;
        }
        public void Crossover(Population population)
        {
            //TODO test
            int populationSize = population.Size;
            for (int i = 0; i < populationSize; i++)
            {
                if (Random.RandomPercent() < crossoverChance)
                {
                    int randomIndex = Random.RandomValue(populationSize);
                    var a = population.Elements.ElementAt(randomIndex);
                    var b = population.Elements.ElementAt((randomIndex + Random.RandomValue(1, populationSize - 1)) % populationSize);

                    var element = population.Elements.ElementAt(i);
                    element = Crossover(a, b);
                }
            }
        }
        Element Crossover(Element a, Element b)
        {
            int length = a.Waypoints.Length;
            int cross1 = Random.RandomValue(length - 1) + 1;
            int cross2 = Random.RandomValue(length - 1) + 1;

            if (cross1 > cross2)
            {
                int tmp = cross1;
                cross1 = cross2;
                cross2 = tmp;
            }

            int[] tab = b.Waypoints;
            int size = tab.Length;
            int[] retval = new int[size];
            for (int i = 0; i < size; i++)
            {
                retval[i] = -1;
            }
            for (int i = 0; i < size; i++)
            {
                if (i >= cross1 && i <= cross2)
                {
                    retval[i] = tab[i];
                }
            }
            for (int i = 0; i < size; i++)
            {
                if (i < cross1 || i > cross2)
                {
                    int tmp = a.Waypoints[i];
                    while (retval.Contains(tmp))
                    {
                        for (int j = 0; j < retval.Length; j++)
                        {
                            if (retval[j] == tmp)
                                tmp = a.Waypoints[j];
                        }

                    }
                    retval[i] = tmp;
                }
            }
            return new Element(retval);
        }
    }
}
