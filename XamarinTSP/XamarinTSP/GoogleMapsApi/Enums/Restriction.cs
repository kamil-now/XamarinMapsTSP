using System.ComponentModel;

namespace XamarinTSP.GoogleMapsApi.Enums
{
    public enum Restriction
    {
        Undefined,
        [Description("tolls")]
        AvoidTolls,
        [Description("highways")]
        AvoidHighways,
        [Description("ferries")]
        AvoidFerries,
        [Description("indoor")]
        AvoidIndoor,
    }
}
