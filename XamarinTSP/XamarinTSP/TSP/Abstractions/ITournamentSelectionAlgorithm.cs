namespace XamarinTSP.TSP.Abstractions
{
    public interface ITournamentSelectionAlgorithm : ISelectionAlgorithm
    {
        int TournamentSize { get; set; }
    }
}
