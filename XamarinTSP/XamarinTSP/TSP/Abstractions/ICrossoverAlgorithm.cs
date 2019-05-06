namespace XamarinTSP.TSP.Abstractions
{
    public interface ICrossoverAlgorithm
    {
        string Name { get; }
        int[] Crossover<T>(Population<T> population, double crossoverChance) where T : IElement;
    }
}