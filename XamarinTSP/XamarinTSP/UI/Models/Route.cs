using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace XamarinTSP.UI.Models
{
    public class Route
    {
        public TimeSpan Time { get; set; } 
        public Distance Distance { get; set; }
        public List<Position> RouteCoordinates { get; set; }
        public Route()
        {
            RouteCoordinates = new List<Position>();
        }
    }
}
