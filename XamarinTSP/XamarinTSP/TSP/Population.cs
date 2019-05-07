using System.Collections.Generic;
using System.Linq;
using XamarinTSP.Common.Extensions;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class Population<TElement> where TElement : IElement
    {
        public int Size => Elements.Count;
        public List<IElement> Elements { get; }

        public IElement Best => Elements.OrderByDescending(x => x.Fitness).First();
        public IElement Worst => Elements.OrderBy(x => x.Fitness).First();
        public double Diversity => Elements.DistinctBy(x => x.Value).Count() / (double)Size;
        
        public Population(List<IElement> elements)
        {
            Elements = elements;
        }
        public void Add(IElement element)
        {
            Elements.Add(element);
        }
        public void Remove(IElement element)
        {
            if (Elements.Contains(element))
                Elements.Remove(element);
        }
        public Population<TElement> Copy() => new Population<TElement>(Elements.Select(x => x.Copy()).ToList());
        public static Population<TElement> GetRandomElementsPopulation(int populationSize, int elementSize)
        {
            var elements = new List<IElement>();
            for (int i = 0; i < populationSize; i++)
            {
                elements.Add(ElementFactory.Create<TElement>(elementSize));
            }
            return new Population<TElement>(elements);
        }
    }
}
