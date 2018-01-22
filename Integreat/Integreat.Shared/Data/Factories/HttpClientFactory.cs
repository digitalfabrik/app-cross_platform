using System;
using System.Net.Http;
using ModernHttpClient;

namespace Integreat.Shared.Data.Factories
{
    internal static class HttpClientFactory
    {
        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetHttpClient(Uri baseAddress)
        {
            var client = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress =  baseAddress
            };
            return client;
        }
    }
}
