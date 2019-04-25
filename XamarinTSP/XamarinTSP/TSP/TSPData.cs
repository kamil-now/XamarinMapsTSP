using System;
using System.Collections.Generic;
using XamarinTSP.Utilities;

namespace XamarinTSP.TSP
{
    public class TSPData : ITSPData
    {
        public IEnumerable<Location> Input { get; }
        public int ElementSize => distanceData.Length;

        private int[][] distanceData;
        private int[][] timeData;
        private bool returnToOrigin;

        public TSPData(IEnumerable<Location> input, int[][] distanceData, int[][] timeData, bool returnToOrigin)
        {
            Input = input;
            this.distanceData = distanceData;
            this.timeData = timeData;
            this.returnToOrigin = returnToOrigin;
        }

        private double CalculateValue(Element element, int[][] data)
        {
            double value = 0;
            int size = data.Length - (returnToOrigin ? 0 : 1);

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
