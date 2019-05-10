using XamarinTSP.TSP.Abstractions;

namespace XamarinTSP.TSP
{
    public class BasicGeneticAlgorithmConfiguration : IBasicGeneticAlgorithmConfiguration
    {
        private ISelectionAlgorithm _selectionAlgorithm;

        public ICrossoverAlgorithm CrossoverAlgorithm { get; set; }
        public ISelectionAlgorithm SelectionAlgorithm
        {
            get => _selectionAlgorithm;
            set
            {
                _selectionAlgorithm = value;
                if(_selectionAlgorithm is ITournamentSelectionAlgorithm tournamentSelection)
                {
                    tournamentSelection.TournamentSize = TournamentSize;
                }
            }

        }
        public IMutationAlgorithm MutationAlgorithm { get; set; }

        public int PopulationSize { get; set; }
        public int TournamentSize { get; set; }
        public double CrossoverChance { get; set; }
        public double MutationChance { get; set; }
        public double ElitismFactor { get; set; }
        public bool Elitism { get; set; }
        public bool MutationBasedOnDiversity { get; set; }
        public bool TimeBasedFitness { get; set; }
        public bool DistanceBasedFitness { get; set; }

        public BasicGeneticAlgorithmConfiguration()
        {
            TournamentSize = 5;
            SelectionAlgorithm = new RouletteSelection();
            CrossoverAlgorithm = new PMXCrossover();
            MutationAlgorithm = new InversionMutation();
            MutationChance = 0.05;
            CrossoverChance = 0.6;
            PopulationSize = 40;
            ElitismFactor = 0.1;

            Elitism = true;
            MutationBasedOnDiversity = false;
            TimeBasedFitness = true;
            DistanceBasedFitness = true;
        }

        public IBasicGeneticAlgorithmConfiguration Copy()
        {
            return new BasicGeneticAlgorithmConfiguration()
            {
                CrossoverAlgorithm = CrossoverAlgorithm,
                SelectionAlgorithm = SelectionAlgorithm,
                MutationAlgorithm = MutationAlgorithm,
                PopulationSize = PopulationSize,
                TournamentSize = TournamentSize,
                CrossoverChance = CrossoverChance,
                MutationChance = MutationChance,
                ElitismFactor = ElitismFactor,
                Elitism = Elitism,
                MutationBasedOnDiversity = MutationBasedOnDiversity,
                TimeBasedFitness = TimeBasedFitness,
                DistanceBasedFitness = DistanceBasedFitness
            };
        }
    }
}
