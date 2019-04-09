namespace XamarinTSP.TSP
{
    public interface ISelectionAlgorithm
    {
        Population Select(Population population, int count);
    }
}
