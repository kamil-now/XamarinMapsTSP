namespace XamarinTSP.TSP
{
    public interface IDistanceData
    {
        int ElementSize { get; }
        void SetValue(Population population);
        void SetFitness(Population population);
        void SetStats(Population population);
    }
}