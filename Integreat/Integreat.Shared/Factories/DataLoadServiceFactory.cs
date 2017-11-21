using Integreat.Utilities;
using ModernHttpClient;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security;
using System.Text;

namespace Integreat.Shared.Factories
{
    class DataLoadServiceFactory
    {
        [SecurityCritical]
        public static IDataLoadService Create()
        {
            var networkServiceSettings = new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Error = (sender, args) => Debug.WriteLine(args)
                    //, TraceWriter = new ConsoleTraceWriter() // debug tracer to see the json input
                }
            };

            var client = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(Constants.IntegreatReleaseUrl)
            };

            return RestService.For<IDataLoadService>(client, networkServiceSettings);
        }
    }
}
