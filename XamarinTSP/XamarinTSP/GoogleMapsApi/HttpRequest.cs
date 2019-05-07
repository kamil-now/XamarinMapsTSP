using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace XamarinTSP.GoogleMapsApi
{
    internal class HttpRequest
    {
        internal static async Task<T> Post<T>(string request, bool tryReauthenticate = true)
        {
            var client = new HttpClient();
            try
            {
                var response = client.PostAsync(request, null).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<T>(responseString);
                    return model;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (tryReauthenticate)
                    {
                        await App.InvokeOnMainThreadAsync(async () => await Post<T>(request, false), 1000);
                    }


                    return default(T);
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
