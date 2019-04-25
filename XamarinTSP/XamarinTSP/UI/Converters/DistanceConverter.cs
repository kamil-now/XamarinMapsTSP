using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinTSP.UI.Converters
{
    public class DistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var retval = "";
            if (value is Distance dist)
            {
                int km = (int)(dist.Meters / 1000);
                int m = (int)(dist.Meters % 1000);
                retval += GetString(km, "km");
                retval += GetString(m, "m");
            }
            return retval;
        }
        public static string GetString(double val, string str)
        {
            if (val >= 1)
                return $"{Math.Round(val)}  {str} ";
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
