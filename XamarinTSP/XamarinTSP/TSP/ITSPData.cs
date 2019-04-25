namespace XamarinTSP.TSP
{
    public interface ITSPData
    {
        int ElementSize { get; }
        void SetValue(Population population);
        void SetFitness(Population population);
        void SetStats(Population population);
    }
}