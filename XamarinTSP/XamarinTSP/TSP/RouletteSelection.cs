using System.Collections.Generic;
using System.Linq;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class RouletteSelection : ISelectionAlgorithm
    {
        public string Name => "Roulette";
        public Population<T> Select<T>(Population<T> population, int count) where T : IElement
        {
            var selected = new List<IElement>();
            var fitnessSum = population.Elements.Sum(x => x.Fitness);
            double[] probs = population.Elements.Select(x => x.Fitness / fitnessSum).ToArray();
            var sm = probs.Sum();
            for (int i = 0; i < count; i++)
            {
                var rand = Random.RandomPercent();

                double sum = 0.0;
                IElement element;
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
            return new Population<T>(selected);
        }
    }
}
