namespace XamarinTSP.TSP.Abstractions
{
    public interface IElement
    {
        double Value { get; set; }
        double Fitness { get; set; }
        int[] Data { get; }
        
        IElement Copy();
    }
}
