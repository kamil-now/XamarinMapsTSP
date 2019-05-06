using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinTSP.UI.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var retval = "";
            if (value is TimeSpan span)
            {
                retval += GetString(span.Days, "d");
                retval += GetString(span.Hours, "h");
                retval += GetString(span.Minutes, "min");
            }
            return retval;
        }
        public static string GetString(int val, string str)
        {
            if (val == 1)
                return val + $" {str} ";
            else if (val > 1)
                return val + $" {str}s ";
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
