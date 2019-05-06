using System.ComponentModel;

namespace XamarinTSP.GoogleMapsApi.Enums
{
    public enum TrafficModel
    {
        Undefined,
        [Description("best_guess")]
        BestGuess,
        [Description("pessimistic")]
        Pessimistic,
        [Description("optimistic")]
        Optimistic
    }
}
