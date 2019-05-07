using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using XamarinTSP.Common.Extensions;
using XamarinTSP.GoogleMapsApi.Abstractions;
using XamarinTSP.GoogleMapsApi.Enums;

namespace XamarinTSP.GoogleMapsApi
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private const int MAX_REQUEST_DESTINATIONS_COUNT = 25;

        public IDistanceMatrixData GetDistanceMatrix(IEnumerable<string> locations, TravelMode travelMode)
        {
            var configuration = new DistanceMatrixRequestConfiguration(locations.ToArray())
            {
                TravelMode = travelMode
            };

            DistanceMatrixResponse response = new DistanceMatrixResponse()
            {
                Origin_Addresses = Array.Empty<string>(),
                Destination_Addresses = Array.Empty<string>(),
                Rows = Array.Empty<Row>(),
            };
            if (locations.Count() < MAX_REQUEST_DESTINATIONS_COUNT)
            {
                var requests = BuildRequests(locations, configuration);
                requests.ForEach(async request =>
                {
                    var res = await HttpRequest.Post<DistanceMatrixResponse>(request);
                    response = response.Merge(res);
                });
            }
            else
            {
                //TODO 
            }
            return new DistanceMatrixData(response);
        }

        public void OpenInGoogleMaps(string[] waypoints)
        {
            var origin = waypoints[0];
            var destination = waypoints[waypoints.Length - 1];
            string str = $"https://www.google.com/maps/dir/?api=1&origin={origin}&destination={destination}&waypoints=";
            var tmp = waypoints.Skip(1).Take(waypoints.Length - 2).ToArray();
            str += string.Join("|", tmp);
            Device.OpenUri(new Uri(str));
        }
        private List<string> BuildRequests(IEnumerable<string> locations, DistanceMatrixRequestConfiguration configuration)
        {
            var count = locations.Count();
            var retval = new List<string>();
            for (int i = 0; i < count; i++)
            {
                retval.Add(BuildSingleRequest(new DistanceMatrixRequestConfiguration(locations.ElementAt(i), locations.ToArray(), configuration)));
            }
            return retval;
        }
        private string BuildSingleRequest(DistanceMatrixRequestConfiguration configuration)
        {
            if (configuration.Destinations == null || configuration.Origins == null)
                throw new ArgumentException("Invalid request configuration");
            if (string.IsNullOrEmpty((Application.Current as App).ApiKey))
                throw new Exception("Missing API key");


            var requestParameters = "";
            requestParameters += $"&key={(Application.Current as App).ApiKey}";
            requestParameters += "&origins=" + string.Join("|", configuration.Origins.Select(x => x.ToString()));
            requestParameters += "&destinations=" + string.Join("|", configuration.Destinations.Select(x => x.ToString()));

            if (configuration.DepartureTime != null && configuration.DepartureTime != DateTime.MinValue)
            {
                requestParameters += "&departure_time=" + ((int)(((DateTime)configuration.DepartureTime).Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            }
            else
            {
                requestParameters += "&departure_time=" + ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            }
            if (configuration.ArrivalTime != null && configuration.ArrivalTime != DateTime.MinValue)
            {
                requestParameters += "&arrival_time=" + ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            }

            if (configuration.Restriction != Restriction.Undefined)
            {

                var flags = (configuration.Restriction as Enum).GetFlags();
                if (flags != null && flags.Count() > 0)
                {
                    requestParameters += "&avoid=" + string.Join("|", flags.Select(flag => (flag as Enum).GetDescription()));
                }
                else
                {
                    requestParameters += "&avoid=" + (configuration.Restriction as Enum).GetDescription();
                }
            }
            if (configuration.TrafficModel != TrafficModel.Undefined)
            {
                requestParameters += "&traffic_model=" + (configuration.TrafficModel as Enum).GetDescription();
            }
            if (configuration.TravelMode != TravelMode.Undefined)
            {
                requestParameters += "&mode=" + (configuration.TravelMode as Enum).GetDescription();
            }
            if (configuration.UnitSystem != UnitSystem.Undefined)
            {
                requestParameters += "&units=" + (configuration.UnitSystem as Enum).GetDescription();
            }

            requestParameters += "&region=" + RegionInfo.CurrentRegion.ThreeLetterISORegionName;

            return $"https://maps.googleapis.com/maps/api/distancematrix/json?{requestParameters}";
        }
    }
}
