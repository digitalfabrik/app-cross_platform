using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Integreat.Shared.Models.Extras.Sprungbrett;
using Newtonsoft.Json;

namespace Integreat.Shared.Utilities
{
    public class SprungbrettParser : ISprungbrettParser
    {
        private readonly HttpClient _client;

        public SprungbrettParser(HttpClient client)
        {
            _client = client;
        }
        public async Task<SprungbrettRootObject> FetchJobOffersAsync(string url)
        {
            try
            {
                var json = await _client.GetStringAsync(new Uri(url));
                return IntegreatJsonConvert.DeserializeObject<SprungbrettRootObject>(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: FetchJobOffers Sprungbrett " + e.Message);
                return null;
            }
        }
    }

    public interface ISprungbrettParser
    {
        Task<SprungbrettRootObject> FetchJobOffersAsync(string url);
    }
}