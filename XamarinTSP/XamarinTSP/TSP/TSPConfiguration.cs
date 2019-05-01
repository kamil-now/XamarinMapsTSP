using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class TSPConfiguration : ITSPConfiguration
    {
        public ICrossoverAlgorithm CrossoverAlgorithm { get; set; }
        public ISelectionAlgorithm SelectionAlgorithm { get; set; }
        public int PopulationSize { get; set; }
        public int TournamentSize { get; set; }
        public double CrossoverChance { get; set; }
        public double MutationChance { get; set; }
        public double ElitismFactor { get; set; }
        public double ElitismChance { get; set; }
        public bool MutationBasedOnDiversity { get; set; }
        public bool TimeBasedFitness { get; set; }
        public bool DistanceBasedFitness { get; set; }

        public TSPConfiguration()
        {
            //defaults
            SelectionAlgorithm = new TournamentSelection();
            (SelectionAlgorithm as ITournamentSelectionAlgorithm).TournamentSize = TournamentSize = 5;
            CrossoverAlgorithm = new PMXCrossover();
            ElitismFactor = 50;
            MutationChance = 0.7;
            CrossoverChance = 0.6;
            PopulationSize = 100;
          

            ElitismFactor = 0.1;
            ElitismChance = 0.5;

            MutationBasedOnDiversity = true;
            TimeBasedFitness = true;

        }
    }
}
