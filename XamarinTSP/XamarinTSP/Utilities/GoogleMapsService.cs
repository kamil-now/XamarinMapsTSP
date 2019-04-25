using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTSP.Utilities
{
    public class GoogleMapsService
    {
        private const int MAX_REQUEST_ELEMENTS_COUNT = 10;
        public async Task<DistanceMatrixResponse> GetDistanceMatrix(DistanceMatrixRequestConfiguration configuration)
        {
            DistanceMatrixResponse response = null;
            if (configuration.Destinations.Count() > MAX_REQUEST_ELEMENTS_COUNT)
            {
                var requests = BuildRequests(configuration);
                requests.ForEach(async request =>
                {
                    var res = await HttpRequest.Post<DistanceMatrixResponse>(request);
                    response.Merge(res);
                });

            }
            else
            {
                var request = BuildSingleRequest(configuration);
                response = await HttpRequest.Post<DistanceMatrixResponse>(request);
                if (response == null)
                {
                    throw new Exception("NULL API RESPONSE");
                }
            }

            return response;
        }
        private List<string> BuildRequests(DistanceMatrixRequestConfiguration configuration)
        {
            var tmp = MAX_REQUEST_ELEMENTS_COUNT;
            var count = configuration.Destinations.Count();
            var retval = new List<string>();
            for (int i = 0; i < count; i += tmp)
            {
                if (i > count)
                    tmp = count - i - MAX_REQUEST_ELEMENTS_COUNT;
                else tmp = i;
                retval.Add(BuildSingleRequest(new DistanceMatrixRequestConfiguration(configuration.Destinations.ToList().GetRange(i, tmp).ToArray(), configuration)));
            }
            return retval;
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
        private string BuildSingleRequest(DistanceMatrixRequestConfiguration configuration)
        {
            //TODO refactor
            if (configuration.Destinations == null || configuration.Origins == null)
                throw new ArgumentException("Invalid request configuration");

            var requestParameters = "";

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
                    requestParameters += name + val;
                }
            }

            requestParameters += $"&key={(Application.Current as App).ApiKey}";
            return $"https://maps.googleapis.com/maps/api/distancematrix/json?{requestParameters}";
        }
    }
}
