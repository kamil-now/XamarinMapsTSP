namespace XamarinTSP.TSP
{
    public interface IDistanceData
    {
        void SetValue(Population population);
        void SetFitness(Population population);
        void SetStats(Population population);
    }
}