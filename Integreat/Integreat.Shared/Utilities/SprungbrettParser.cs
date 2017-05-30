using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Integreat.Shared.Models.Sprungbrett;
using Newtonsoft.Json;

namespace Integreat.Shared.Utilities
{
    public partial class SprungbrettTemp
    {
        public async Task<SprungbrettRootObject> FetchJobOffersAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
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