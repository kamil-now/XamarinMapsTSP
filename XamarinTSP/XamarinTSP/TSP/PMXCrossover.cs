using System;
using System.Linq;
using XamarinTSP.Utilities;

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
            int populationSize = population.Size;
            for (int i = 0; i < populationSize; i++)
            {
                if (Helper.RandomPercent() < crossoverChance)
                {
                    int randomIndex = Helper.Random(populationSize);
                    var a = population.Elements.ElementAt(randomIndex);
                    var b = population.Elements.ElementAt((randomIndex + Helper.Random(1, populationSize - 1)) % populationSize);

                    var element = population.Elements.ElementAt(i);
                    element = Crossover(a, b);
                }
            }
        }
        Element Crossover(Element a, Element b)
        {
            //if (a.Data.Length != b.Data.Length)
            //    throw new Exception("DISTANCE DATA EXCEPTION");
            int length = a.Waypoints.Length;
            int cross1 = Helper.Random(length - 1) + 1;
            int cross2 = Helper.Random(length - 1) + 1;

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
