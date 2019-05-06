namespace XamarinTSP.TSP.Abstractions
{
    public interface IRouteElement : IElement
    {
        double DistanceValue { get; set; }
        double DurationValue { get; set; }

    }
}
