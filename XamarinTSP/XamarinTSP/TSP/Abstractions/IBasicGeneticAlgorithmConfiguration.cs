namespace XamarinTSP.TSP.Abstractions
{
    public interface IBasicGeneticAlgorithmConfiguration
    {
        ICrossoverAlgorithm CrossoverAlgorithm { get; set; }
        ISelectionAlgorithm SelectionAlgorithm { get; set; }
        IMutationAlgorithm MutationAlgorithm { get; set; }

        int PopulationSize { get; set; }
        int TournamentSize { get; set; }
        double CrossoverChance { get; set; }
        double MutationChance { get; set; }
        double ElitismFactor { get; set; }
        bool Elitism { get; set; }
        bool MutationBasedOnDiversity { get; set; }
        bool TimeBasedFitness { get; set; }
        bool DistanceBasedFitness { get; set; }

        IBasicGeneticAlgorithmConfiguration Copy();
    }
}
