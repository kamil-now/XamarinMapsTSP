namespace XamarinTSP.TSP.Abstractions
{
    public interface ISelectionAlgorithm
    {
        string Name { get; }
        Population Select(Population population, int count);
    }
}
