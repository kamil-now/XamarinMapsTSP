namespace XamarinTSP.TSP.Abstractions
{
    public interface ITSPConfiguration
    {
        ICrossoverAlgorithm CrossoverAlgorithm { get; set; }
        ISelectionAlgorithm SelectionAlgorithm { get; set; }
        int PopulationSize { get; set; }
        int TournamentSize { get; set; }
        double CrossoverChance { get; set; }
        double MutationChance { get; set; }
        double ElitismFactor { get; set; }
        double ElitismChance { get; set; }
        bool MutationBasedOnDiversity { get; set; }
        bool TimeBasedFitness { get; set; }
        bool DistanceBasedFitness { get; set; }
    }
}
