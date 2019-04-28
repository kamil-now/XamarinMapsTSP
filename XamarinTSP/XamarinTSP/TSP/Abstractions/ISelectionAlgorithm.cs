namespace XamarinTSP.TSP.Common.Abstractions
{
    public interface ISelectionAlgorithm
    {
        Population Select(Population population, int count);
    }
}
