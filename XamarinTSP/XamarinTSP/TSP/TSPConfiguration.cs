namespace XamarinTSP.TSP
{
    public class TSPConfiguration
    {
        public ICrossoverAlgorithm CrossoverAlgorithm { get; set; }
        public ISelectionAlgorithm SelectionAlgorithm { get; set; }
        public int PopulationSize { get; set; }
        public double CrossoverChance { get; set; }
        public double MutationChance { get; set; }
        public double ElitismFactor { get; set; }
        public double ElitismChance { get; set; }
        public bool ReturnToOrigin { get; set; }
        public bool MutationBasedOnDiversity { get; set; }

        public TSPConfiguration()
        {

            ElitismFactor = 50;
            MutationChance = 0.7;
            CrossoverChance = 0.6;
            PopulationSize = 100;

            ElitismFactor = 0.1;
            ElitismChance = 0.5;

            ReturnToOrigin = true;
            MutationBasedOnDiversity = true;
            SelectionAlgorithm = new TournamentSelection(5);
            CrossoverAlgorithm = new PMXCrossover(0.6);

        }
        public bool Validate()
        {
            //TODO
            return true;
        }
    }
}
