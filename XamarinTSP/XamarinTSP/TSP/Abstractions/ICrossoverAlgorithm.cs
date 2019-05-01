namespace XamarinTSP.TSP.Abstractions
{
    public interface ICrossoverAlgorithm
    {
        string Name { get; }
        void Crossover(Population population, double crossoverChance);
    }
}