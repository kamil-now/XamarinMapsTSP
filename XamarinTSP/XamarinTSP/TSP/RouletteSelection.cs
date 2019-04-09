using System.Collections.Generic;
using System.Linq;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class RouletteSelection : ISelectionAlgorithm
    {
        public Population Select(Population population, int count)
        {
            var selected = new List<Element>();
            var fitnessSum = population.Elements.Sum(x => x.Fitness);
            double[] probs = population.Elements.Select(x => x.Fitness / fitnessSum).ToArray();
            var sm = probs.Sum();
            for (int i = 0; i < count; i++)
            {
                var rand = Helper.RandomPercent();

                double sum = 0.0;
                Element element;
                for (int k = 0; ; k++)
                {
                    sum += probs[k];
                    if (rand < sum)
                    {
                        element = population.Elements.ElementAt(k);
                        break;
                    }
                    if (k == probs.Length - 1)
                        k = 0;
                }
                selected.Add(element.Copy());
            }
            selected.Add(population.Best);
            return new Population(selected);
        }
    }
}
