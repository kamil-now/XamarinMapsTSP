using System.Collections.Generic;
using System.Linq;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class Population
    {
        public int Size => Elements.Count;
        public List<Element> Elements { get; }

        public Element Best => Elements.OrderByDescending(x => x.Fitness).First();
        public Element Worst => Elements.OrderBy(x => x.Fitness).First();
        public double Diversity => Elements.DistinctBy(x => x.DistanceValue).Count() / (double)Size;
        /// <summary>
        /// Randomized elements population
        /// </summary>
        public Population(int populationSize, int elementSize)
        {
            Elements = new List<Element>();
            for (int i = 0; i < populationSize; i++)
            {
                Add(new Element(elementSize));
            }
        }
        public Population(List<Element> elements)
        {
            Elements = elements;
        }
        public void Add(Element element)
        {
            Elements.Add(element);
        }
        public void Remove(Element element)
        {
            if (Elements.Contains(element))
                Elements.Remove(element);
        }
        public Population Copy() => new Population(Elements.Select(x => x.Copy()).ToList());
    }
}
