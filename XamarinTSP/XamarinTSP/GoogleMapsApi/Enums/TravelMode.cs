using System.ComponentModel;

namespace XamarinTSP.GoogleMapsApi.Enums
{
    public enum TravelMode
    {
        Undefined,
        [Description("driving")]
        Driving,
        [Description("walking")]
        Walking,
        [Description("bicycling")]
        Bicycling
    }
}
