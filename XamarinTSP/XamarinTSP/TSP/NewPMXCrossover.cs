using System.Linq;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class NewPMXCrossover : ICrossoverAlgorithm
    {
        double crossoverChance;
        public NewPMXCrossover(double crossoverChance)
        {
            this.crossoverChance = crossoverChance;
        }
        public void Crossover(Population population)
        {
            int populationSize = population.Size;


            for (int k = 0; k < populationSize / 2; k++)
            {
                int randIndexA = Helper.Random(0, populationSize);
                int randIndexB = Helper.Random(0, populationSize);
                var a = population.Elements.ElementAt(randIndexA).Copy();
                var b = population.Elements.ElementAt(randIndexB).Copy();

                if (Helper.RandomPercent() < crossoverChance)
                {
                    var tmpA = a.Copy();
                    var tmpB = b.Copy();
                    a = Crossover(tmpA, tmpB);
                    b = Crossover(tmpB, tmpA);
                }
                var tmp = population.Elements.ElementAt(k);
                tmp = a;

                var tmp2 = population.Elements.ElementAt(populationSize - k - 1);
                tmp2 = b;
            }
        }
        Element Crossover(Element a, Element b)
        {
            int length = a.Data.Length;
            int cross1 = Helper.Random(length - 1) + 1;
            int cross2 = Helper.Random(length - 1) + 1;

            if (cross1 > cross2)
            {
                int tmp = cross1;
                cross1 = cross2;
                cross2 = tmp;
            }

            int[] tab = b.Data;
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
                    int tmp = a.Data[i];
                    while (retval.Contains(tmp))
                    {
                        for (int j = 0; j < retval.Length; j++)
                        {
                            if (retval[j] == tmp)
                                tmp = a.Data[j];
                        }

                    }
                    retval[i] = tmp;
                }
            }
            return new Element(retval);
        }
    }
}
