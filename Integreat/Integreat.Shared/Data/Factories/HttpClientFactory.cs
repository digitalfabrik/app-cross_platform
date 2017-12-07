using System;
using System.Net.Http;
using Integreat.Utilities;
using ModernHttpClient;

namespace Integreat.Shared.Data.Factories
{
    internal static class HttpClientFactory
    {
        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress =  new Uri(Constants.IntegreatReleaseUrl)
            };
            return client;
        }
    }
}
