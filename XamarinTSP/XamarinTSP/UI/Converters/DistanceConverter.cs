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
                var val = Math.Round(dist.Meters / 1000, 2);

                if (val > 0)
                {
                    retval += $"{val} km";
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
