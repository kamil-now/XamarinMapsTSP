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
        public int MAX_REQUEST_DESTINATIONS_COUNT => 25;
        private IDistanceMatrixRequestConfiguration _activeDistanceMatrixConfiguration;
        public GoogleMapsService(IDistanceMatrixRequestConfiguration activeDistanceMatrixConfiguration)
        {
            _activeDistanceMatrixConfiguration = activeDistanceMatrixConfiguration;
        }
        public IDistanceMatrixData GetDistanceMatrix(IEnumerable<string> locations, TravelMode travelMode)
        {
            _activeDistanceMatrixConfiguration.Destinations = _activeDistanceMatrixConfiguration.Origins = locations.ToArray();
            _activeDistanceMatrixConfiguration.TravelMode = _activeDistanceMatrixConfiguration.TravelMode;

             DistanceMatrixResponse response = new DistanceMatrixResponse()
            {
                Origin_Addresses = Array.Empty<string>(),
                Destination_Addresses = Array.Empty<string>(),
                Rows = Array.Empty<Row>(),
            };
            if (locations.Count() < MAX_REQUEST_DESTINATIONS_COUNT)
            {
                var requests = BuildRequests(locations, _activeDistanceMatrixConfiguration);
                requests.ForEach(async request =>
                {
                    var res = await HttpRequest.Post<DistanceMatrixResponse>(request);
                    response = response.Merge(res);
                });
            }
            else
            {
                throw new ArgumentException("MAX_REQUEST_DESTINATIONS_COUNT value exceeded");
            }
            return new DistanceMatrixData(response);
        }

        public void OpenInGoogleMaps(string[] waypoints)
        {
            string str = "https://www.google.com/maps/dir/";
            str += string.Join("/",waypoints);
            Device.OpenUri(new Uri(str));
        }
        private List<string> BuildRequests(IEnumerable<string> locations, IDistanceMatrixRequestConfiguration configuration)
        {
            var count = locations.Count();
            var retval = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var config = configuration.Copy();
                config.Origin = locations.ElementAt(i);
                retval.Add(BuildSingleRequest(config));
            }
            return retval;
        }
        private string BuildSingleRequest(IDistanceMatrixRequestConfiguration configuration)
        {
            if (configuration.Destinations == null || string.IsNullOrEmpty(configuration.Origin))
                throw new ArgumentException("Invalid request configuration");
            if (string.IsNullOrEmpty((Application.Current as App).ApiKey))
                throw new Exception("Missing API key");

            configuration.Origins = new[] { configuration.Origin };

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

            requestParameters += "&units=" + UnitSystem.Metric.GetDescription();
            requestParameters += "&region=" + RegionInfo.CurrentRegion.ThreeLetterISORegionName;

            return $"https://maps.googleapis.com/maps/api/distancematrix/json?{requestParameters}";
        }
    }
}
