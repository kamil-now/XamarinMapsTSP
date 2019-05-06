using System;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class InversionMutation : IMutationAlgorithm
    {
        public string Name => "Inversion";

        public void Mutate(IElement element)
        {
            var data = element.Data;
            int gen1 = Random.RandomValue(0, data.Length);
            int gen2 = Random.RandomValue(0, data.Length);

            if (gen1 > gen2)
            {
                int tmp = gen2;
                gen2 = gen1;
                gen1 = tmp;
            }

            int[] tmpArr = new int[gen2 - gen1];

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                tmpArr[x] = data[p];
            }

            Array.Reverse(tmpArr);

            for (int p = gen1, x = 0; p < gen2; p++, x++)
            {
                data[p] = tmpArr[x];
            }
        }
    }
}
