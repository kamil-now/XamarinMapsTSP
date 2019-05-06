namespace XamarinTSP.TSP.Abstractions
{
    public interface ISelectionAlgorithm
    {
        string Name { get; }
        Population<T> Select<T>(Population<T> population, int count) where T : IElement;
    }
}
