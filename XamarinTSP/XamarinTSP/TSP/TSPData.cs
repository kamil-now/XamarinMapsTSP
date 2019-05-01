using System;
using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class TSPData : ITSPData
    {
        public IEnumerable<object> Input { get; }
        public int ElementSize => distanceData.Length;

        private int[][] distanceData;
        private int[][] timeData;

        public TSPData(IEnumerable<object> input, int[][] distanceData, int[][] timeData)
        {
            Input = input;
            this.distanceData = distanceData;
            this.timeData = timeData;
        }

        private double CalculateValue(Element element, int[][] data)
        {
            //TODO test
            double value = 0;
            int size = data.Length;

            for (int i = 0; i < size; i++)
            {
                int row, column, tmp;
                row = element.Waypoints[i];
                tmp = (i + 1) % size;
                column = element.Waypoints[tmp];
                value += data[row][column];
            }
            return value;
        }

        private double CalculateFitness(Element element)
        {
            return 1 / element.DistanceValue + 1 / element.TimeValue;
        }
        public void SetValue(Population population)
        {
            foreach (var element in population.Elements)
            {
                element.DistanceValue = CalculateValue(element, distanceData);
                element.TimeValue = CalculateValue(element, timeData);
            }
        }
        public void SetFitness(Population population)
        {
            foreach (var element in population.Elements)
            {
                if (element.DistanceValue <= 0)
                    throw new Exception("SET VALUE FIRST");
                element.Fitness = CalculateFitness(element);
            }
        }
        public void SetStats(Population population)
        {
            foreach (var element in population.Elements)
            {
                element.DistanceValue = CalculateValue(element, distanceData);
                element.TimeValue = CalculateValue(element, timeData);
                element.Fitness = CalculateFitness(element);
            }
        }
    }
}
