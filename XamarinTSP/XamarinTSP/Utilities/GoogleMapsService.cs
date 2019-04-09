using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTSP.Utilities
{
    public class GoogleMapsService
    {
        public async Task<DistanceMatrixResponse> GetDistanceMatrix(DistanceMatrixConfiguration configuration)
        {
            var parameters = BuildRequestParameters(configuration);
            return await HttpRequest.Post<DistanceMatrixResponse>($@"https://maps.googleapis.com/maps/api/distancematrix/json?{parameters}");
        }

        public void OpenInGoogleMaps(string[] waypoints)
        {
            var origin = waypoints[0];
            var destination = waypoints[waypoints.Length - 1];
            string str = $"https://www.google.com/maps/dir/?api=1&origin={origin}&destination={destination}&waypoints=";
            str += string.Join("%7C", waypoints.Skip(1).Take(waypoints.Length - 2));
            Device.OpenUri(new Uri(str));
        }
        private string BuildRequestParameters(DistanceMatrixConfiguration configuration)
        {
            if (configuration.Destinations == null || configuration.Origins == null)
                throw new ArgumentException("Invalid request configuration");

            var request = "";

            var props = configuration.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(configuration, null);
                if (value == null)
                    continue;
                var name = $"&{Helper.GetDescription(prop)}=";
                var val = "";
                if (value.GetType().IsEnum)
                {
                    var flags = Helper.GetFlags(value as Enum);
                    if (flags != null && flags.Count() > 0)
                    {
                        val += string.Join("%7C", flags.Select(flag => Helper.GetDescription(flag)));
                    }
                    else
                    {
                        val += Helper.GetDescription(value as Enum);
                    }
                }
                else if (value.GetType().IsArray)
                {
                    var array = value as object[];
                    val += string.Join("%7C", array.Select(x => x.ToString()));
                }
                else if (value.GetType() == typeof(DateTime))
                {
                    val += Helper.GetTimestamp((DateTime)value);
                }
                else
                {
                    val += value.ToString();
                }
                if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(name))
                {
                    request += name + val;
                }
            }

            request += $"&key={(Application.Current as App).ApiKey}";
            return request;
        }
    }
}
