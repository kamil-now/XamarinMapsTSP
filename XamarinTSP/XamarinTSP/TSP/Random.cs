using System.Collections.Generic;

namespace XamarinTSP.TSP
{
    public static class Random
    {
        private static System.Random random = new System.Random();
        public static double RandomPercent() => random.NextDouble();
        public static int RandomValue(int val) => random.Next(val);
        public static int RandomValue(int val, int max) => random.Next(val, max);
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
