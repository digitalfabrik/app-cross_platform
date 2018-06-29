using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Integreat.Shared.Utilities
{
    public class Parser : IParser
    {
        private readonly HttpClient _client;

        public Parser(HttpClient client)
        {
            _client = client;
        }
        public async Task<T> FetchAsync<T>(string url)
        {
            try
            {
                var json = await _client.GetStringAsync(new Uri(url));
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: FetchJobOffers " + e.Message);
                return default(T);
            }
        }
    }

    public interface IParser
    {
        Task<T> FetchAsync<T>(string url);
    }
}