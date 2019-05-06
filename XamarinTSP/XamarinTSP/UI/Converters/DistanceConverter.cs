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
                var km = dist.Meters / 1000;
                var m = Math.Round(dist.Meters % 1000,2);
                
                if (km > 0 || m > 0)
                {
                    retval += $"{km}{System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator}{m}";
                }
            }
            return retval;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
