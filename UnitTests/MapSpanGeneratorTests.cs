using Xamarin.Forms.Maps;
using XamarinTSP.UI.ViewModels;
using XamarinTSP.Utilities;
using Xunit;

namespace UnitTests
{
    public class MapSpanGeneratorTests
    {
        [Fact]
        public void MapSpanGenerator_MeasureDistance()
        {
            var a = new Position(50.1234, 51.4321);
            var b = new Position(50.4321, 51.1234);
            var result = MapSpanGenerator.MeasureDistanceKm(a, b);
            Assert.Equal(40.74, result, 2);
        }
    }
}
