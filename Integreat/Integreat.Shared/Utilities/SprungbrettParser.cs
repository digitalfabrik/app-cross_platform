using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Integreat.Shared.Models.Extras.Sprungbrett;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Integreat.Shared.Utilities
{
    public class SprungbrettParser
    {
        public async Task<SprungbrettRootObject> FetchJobOffersAsync(string url)
        {
            try
            {
                using (var client = new HttpClient(new NativeMessageHandler())) // todo use client via DI from autofac
                {
                    var json = await client.GetStringAsync(new Uri(url));
                    return JsonConvert.DeserializeObject<SprungbrettRootObject>(json);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: FetchJobOffers Sprungbrett " + e.Message);
                return null;
            }
        }
    }
}