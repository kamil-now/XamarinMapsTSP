using System.Collections.Generic;
using System.Linq;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class Element : IElement
    {
        public double Value { get; set; } = -1;
        public double Fitness { get; set; } = -1;
        public int[] Data { get; private set; }

        public Element(int size) : this(Random.GetRandomData(size)) { }

        public Element(IEnumerable<int> data)
        {
            Data = data.ToArray();
        }
        public Element(int[] data)
        {
            Data = data;
        }

        private Element(IEnumerable<int> data, double value, double fitness)
        {
            Data = data.ToArray();
            Value = value;
            Fitness = fitness;
        }
        public IElement Copy() => new Element(Data, Value, Fitness);
    }
}