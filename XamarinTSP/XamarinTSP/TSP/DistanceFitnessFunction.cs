using System.Collections.Generic;
using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class DistanceFitnessFunction : BaseFitnessFunction, IDistanceFitnessFunction
    {
        public override string Name => "Distance";
        public DistanceFitnessFunction(IEnumerable<object> input, int[][] data) : base(input, data)
        {
        }
        protected override double CalculateFitness(IElement element) => 1 / element.Value;
    }
}
