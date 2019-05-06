namespace XamarinTSP.GoogleMapsApi.Abstractions
{
    public interface IDistanceMatrixData
    {
        int[][] DurationMatrix { get; }
        int[][] DistanceMatrix { get; }
        string[] Waypoints { get; }
    }
}
