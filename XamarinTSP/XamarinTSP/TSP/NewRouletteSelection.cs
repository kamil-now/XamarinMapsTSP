using System;
using System.Collections.Generic;
using System.Linq;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class NewRouletteSelection : ISelectionAlgorithm
    {
        public Population Select(Population population, int count)
        {
            int size = population.Size;
            var selected = new List<Element>();
            var roulette = new List<Element>();
            var sorted = population.Elements.OrderBy(x => x.DistanceValue).ToList();

            for (int i = count - 1; i > 0; i--)
            {

                var x = Math.Round((double)count / 10);
                for (int j = 0; j < x; j++)
                {
                    roulette.Add(sorted.ElementAt(i));

                }
            }


            while (selected.Count < count)
            {
                int random1 = Helper.Random(0, roulette.Count);
                int random2 = Helper.Random(0, roulette.Count);

                if (random1 < random2)
                {
                    selected.Add(roulette.ElementAt(random1));
                }
            }

            return new Population(selected);
        }
    }
}
