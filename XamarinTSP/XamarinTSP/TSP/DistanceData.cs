using System;

namespace XamarinTSP.TSP
{
    public class DistanceData : IDistanceData
    {
        public int ElementSize => distanceData.Length;
        int[][] distanceData;
        bool returnToOrigin;

        public DistanceData(int[][] data, bool returnToOrigin)
        {
            this.distanceData = data;
            this.returnToOrigin = returnToOrigin;
        }
        double CalculateValue(Element element)
        {
            double value = 0;
            int size = distanceData.Length - (returnToOrigin ? 0 : 1);

            for (int i = 0; i < size; i++)
            {
                int row, column, tmp;
                row = element.Data[i];
                tmp = (i + 1) % size;
                column = element.Data[tmp];
                value += distanceData[row][column];
            }
            return value;
        }
        double CalculateFitness(Element element)
        {
            return 1 / element.Value;
        }
        public void SetValue(Population population)
        {
            foreach (var element in population.Elements)
            {
                element.Value = CalculateValue(element);
            }
        }
        public void SetFitness(Population population)
        {
            foreach (var element in population.Elements)
            {
                if (element.Value <= 0)
                    throw new Exception("SET VALUE FIRST");
                element.Fitness = CalculateFitness(element);
            }
        }
        public void SetStats(Population population)
        {
            foreach (var element in population.Elements)
            {
                element.Value = CalculateValue(element);
                element.Fitness = CalculateFitness(element);
            }
        }
    }
}
