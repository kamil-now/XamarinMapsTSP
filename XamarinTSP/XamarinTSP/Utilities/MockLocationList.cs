using Xamarin.Forms.Internals;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public static class MockLocationList
    {
        public static LocationList List(IGeolocationService geolocation)
        {
            var list = new LocationList()
            {
                Locations = new System.Collections.ObjectModel.ObservableCollection<Location>()
                {
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.252711, 19.015991) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.255272, 19.035315) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.247866, 19.025751) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.261930, 19.008051) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.241313, 19.017488) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.247246, 18.997965) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.254237, 19.019152) }
                }
            };
            return list;
        }
    }
}
