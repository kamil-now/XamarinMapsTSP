using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;

namespace XamarinTSP.UI.Utilities
{
    internal static class MapSpanGenerator
    {
        private const int EARTH_RADIUS_KM = 6371;
        public static MapSpan Generate(IEnumerable<Position> points)
        {
            var result = Calculate(points);

            return MapSpan.FromCenterAndRadius(result.center, Distance.FromKilometers(result.radius));
        }
        public static MapSpan Generate(Position point)
        {
            return MapSpan.FromCenterAndRadius(point, Distance.FromKilometers(5));
        }
        public static double MeasureDistanceKm(Position a, Position b)
        {
            var x = Math.Pow(Math.Sin(DegreeToRadian(b.Latitude - a.Latitude) / 2), 2)
                  + Math.Pow(Math.Sin(DegreeToRadian(b.Longitude - a.Longitude) / 2), 2) * Math.Cos(DegreeToRadian(a.Latitude)) * Math.Cos(DegreeToRadian(b.Latitude));

            var c = 2 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1 - x));
            return EARTH_RADIUS_KM * c;
        }
        private static (Position center, double radius) Calculate(IEnumerable<Position> points)
        {
            var maxX = points.Aggregate((a, b) => a.Latitude > b.Latitude ? a : b);
            var maxY = points.Aggregate((a, b) => a.Longitude > b.Longitude ? a : b);
            var minX = points.Aggregate((a, b) => a.Latitude < b.Latitude ? a : b);
            var minY = points.Aggregate((a, b) => a.Longitude < b.Longitude ? a : b);
            var x = MeasureDistanceKm(maxX, minX);
            var y = MeasureDistanceKm(maxY, minY);
            Position s;
            if (x > y)
            {
                s = new Position((maxX.Latitude + minX.Latitude) / 2, (maxX.Longitude + minX.Longitude) / 2);
            }
            else
            {
                s = new Position((maxY.Latitude + minY.Latitude) / 2, (maxY.Longitude + minY.Longitude) / 2);
            }
            return (s, Math.Sqrt(x * x + y * y) / 2);
        }

        private static double DegreeToRadian(double degree) => degree * Math.PI / 180;
    }
}
