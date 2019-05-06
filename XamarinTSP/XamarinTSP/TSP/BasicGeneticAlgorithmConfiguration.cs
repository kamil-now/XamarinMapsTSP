using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class BasicGeneticAlgorithmConfiguration : IBasicGeneticAlgorithmConfiguration
    {
        public ICrossoverAlgorithm CrossoverAlgorithm { get; set; }
        public ISelectionAlgorithm SelectionAlgorithm { get; set; }
        public IMutationAlgorithm MutationAlgorithm { get; set; }

        public int PopulationSize { get; set; }
        public int TournamentSize { get; set; }
        public double CrossoverChance { get; set; }
        public double MutationChance { get; set; }
        public double ElitismFactor { get; set; }
        public bool MutationBasedOnDiversity { get; set; }
        public bool TimeBasedFitness { get; set; }
        public bool DistanceBasedFitness { get; set; }

        public BasicGeneticAlgorithmConfiguration()
        {
            //defaults
            SelectionAlgorithm = new TournamentSelection()
            {
                TournamentSize = 5
            };
            CrossoverAlgorithm = new PMXCrossover();
            MutationAlgorithm = new InversionMutation();
            MutationChance = 0.05;
            CrossoverChance = 0.6;
            PopulationSize = 40;
            ElitismFactor = 0.1;

            MutationBasedOnDiversity = true;
            TimeBasedFitness = true;

        }
    }
}
