using System;
using System.Collections.Generic;
using System.Linq;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class Element
    {
        public double DistanceValue { get; set; } = -1;
        public double Fitness { get; set; } = -1;
        public double TimeValue { get; set; }
        public int PassCount { get; set; }
        public int[] Waypoints { get; private set; }

        public Element(int size) : this(Helper.GetRandomData(size)) { }

        public Element(IEnumerable<int> data)
        {
            Waypoints = data.ToArray();
        }
        public Element(int[] data)
        {
            Waypoints = data;
        }

        private Element(IEnumerable<int> data, double distanceValue, double timeValue, double fitness, int passCount)
        {
            Waypoints = data.ToArray();
            DistanceValue = distanceValue;
            TimeValue = timeValue;
            Fitness = fitness;
            PassCount = passCount;
        }
        public Element Copy() => new Element(Waypoints, DistanceValue, TimeValue, Fitness, PassCount);

        public void Mutate()
        {
            int gen1 = Helper.Random(0, Waypoints.Length);
            int gen2 = Helper.Random(0, Waypoints.Length);

            if (gen1 > gen2)
            {
                int tmp = gen2;
                gen2 = gen1;
                gen1 = tmp;
            }

            int[] tmpArr = new int[gen2 - gen1];

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                tmpArr[x] = Waypoints[p];
            }

            Array.Reverse(tmpArr);

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                Waypoints[p] = tmpArr[x];
            }

        }
    }
}