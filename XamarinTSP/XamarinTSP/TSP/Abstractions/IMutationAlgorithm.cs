namespace XamarinTSP.TSP.Abstractions
{
    public interface IMutationAlgorithm
    {
        string Name { get; }
        void Mutate(IElement data);
    }
}
