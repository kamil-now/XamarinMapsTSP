using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class DurationFitnessFunction : BaseFitnessFunction, IDurationFitnessFunction
    {
        public override string Name => "Duration";
        public DurationFitnessFunction(IEnumerable<object> input, int[][] data) : base(input, data)
        {
        }
        protected override double CalculateFitness(IElement element) => 1 / element.Value * 10000;
    }
}