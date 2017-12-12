using System;
using System.Net.Http;
using Integreat.Utilities;
using ModernHttpClient;
using Xamarin.Forms;

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
            HttpClient client;
            if (Device.RuntimePlatform == Device.Android)
            {
                client = new HttpClient(new NativeMessageHandler())
                {
                    BaseAddress = new Uri(Constants.IntegreatReleaseUrl)
                };
            }
            else
            {
                client = new HttpClient
                {
                    BaseAddress = new Uri(Constants.IntegreatReleaseUrl)
                };
            }
            
            return client;
        }
    }
}
