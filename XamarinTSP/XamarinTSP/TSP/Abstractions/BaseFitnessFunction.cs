using System;
using System.Collections.Generic;

namespace XamarinTSP.TSP.Abstractions
{
    public abstract class BaseFitnessFunction : IFitnessFunction
    {
        public abstract string Name { get; }
        public IEnumerable<object> Input { get; }
        public int ElementSize => data.Length;

        protected int[][] data;

        public BaseFitnessFunction(IEnumerable<object> input, int[][] data)
        {
            Input = input;
            this.data = data;
        }
        protected abstract double CalculateFitness(IElement element);
        protected double CalculateValue(IElement element, int[][] data)
        {
            double value = 0;
            int size = data.Length;

            for (int i = 0; i < size; i++)
            {
                int row, column, tmp;
                row = element.Data[i];
                tmp = (i + 1) % size;
                column = element.Data[tmp];
                value += data[row][column];
            }
            return value;
        }
        public virtual void SetFitness<T>(Population<T> population) where T : IElement
        {
            foreach (var element in population.Elements)
            {
                element.Value = CalculateValue(element, data);
                element.Fitness = CalculateFitness(element);
            }
            
        }
    }
}

