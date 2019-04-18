using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTSP.Utilities
{
    public class GoogleMapsService
    {
        public async Task<DistanceMatrixResponse> GetDistanceMatrix(DistanceMatrixRequestConfiguration configuration)
        {
            var parameters = BuildRequestParameters(configuration);
            var response = await HttpRequest.Post<DistanceMatrixResponse>($@"https://maps.googleapis.com/maps/api/distancematrix/json?{parameters}");
            if (response == null)
            {
                throw new Exception("NULL API RESPONSE");
            }
            return response;
        }

        public void OpenInGoogleMaps(IEnumerable<Location> locations)
        {
            var waypoints = locations.Select(x => $"{x.Position.Latitude}, {x.Position.Longitude}").ToArray();
            var origin = waypoints[0];
            var destination = waypoints[waypoints.Length - 1];
            string str = $"https://www.google.com/maps/dir/?api=1&origin={origin}&destination={destination}&waypoints=";
            str += string.Join("|", waypoints.Skip(1).Take(waypoints.Length - 2));
            Device.OpenUri(new Uri(str));
        }
        private string BuildRequestParameters(DistanceMatrixRequestConfiguration configuration)
        {
            //TODO refactor
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
                        val += string.Join("|", flags.Select(flag => Helper.GetDescription(flag)));
                    }
                    else
                    {
                        val += Helper.GetDescription(value as Enum);
                    }
                }
                else if (value.GetType().IsArray)
                {
                    var array = value as object[];
                    val += string.Join("|", array.Select(x => x.ToString()));
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
