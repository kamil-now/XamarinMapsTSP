using Xamarin.Forms.Maps;
using XamarinTSP.UI.ViewModels;
using Xunit;

namespace UnitTests
{
    public class CustomMapTests
    {
        [Fact]
        public void CustomMap_MeasureDistance()
        {
            CustomMap map = new CustomMap();
            var a = new Position(50.1234, 51.4321);
            var b = new Position(50.4321, 51.1234);
            var result = map.MeasureDistance(a, b);
            Assert.Equal(40.74, result, 2);
        }
    }
}
