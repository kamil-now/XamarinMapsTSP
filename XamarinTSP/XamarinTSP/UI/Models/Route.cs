using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace XamarinTSP.UI.Models
{
    public class Route
    {
        public List<Position> RouteCoordinates { get; set; }
        public Route()
        {
            RouteCoordinates = new List<Position>();
        }
    }
}
