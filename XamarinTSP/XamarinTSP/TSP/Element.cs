using System;
using System.Collections.Generic;
using System.Linq;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class Element
    {
        public string DataString => string.Join("-", Data);
        public string Description => $"\tVALUE {Value}\n\t FITNESS {Fitness.ToString("0.############")} \tDATA:\t\n {DataString}";

        public double Value { get; set; } = -1;
        public double Fitness { get; set; } = -1;
        public int PassCount { get; set; }
        public int[] Data { get; private set; }

        public Element(int size) : this(Helper.GetRandomData(size)) { }

        public Element(IEnumerable<int> data)
        {
            Data = data.ToArray();
        }
        public Element(int[] data)
        {
            Data = data;
        }
        Element(IEnumerable<int> data, double value, double fitness, int passCount)
        {
            Data = data.ToArray();

            Value = value;

            Fitness = fitness;

            PassCount = passCount;

        }
        public Element Copy() => new Element(Data, Value, Fitness, PassCount);

        public void Mutate()
        {
            int gen1 = Helper.Random(0, Data.Length);
            int gen2 = Helper.Random(0, Data.Length);

            if (gen1 > gen2)
            {
                int tmp = gen2;
                gen2 = gen1;
                gen1 = tmp;
            }

            int[] tmpArr = new int[gen2 - gen1];

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                tmpArr[x] = Data[p];
            }

            Array.Reverse(tmpArr);

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                Data[p] = tmpArr[x];
            }

        }
    }
}