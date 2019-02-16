using System.Diagnostics;
using System.Net.Http;
using Integreat.Data.Loader.Targets;
using Integreat.Model;
using Newtonsoft.Json;
using Refit;
using Xunit;

namespace Integreat.Data.Test
{
    public class DataLoadServiceTest
    {
        [Fact]
        public void GetDisclaimerTest()
        {
            var dataService = new DisclaimerDataLoader(CreateDataLoadService(new HttpClient()));
            Language lastLoadedLanguage = null;
            Location lastLoadedLocation = null;

            string result = null;

            Assert.NotNull(result);
        }

        private static IDataLoadService CreateDataLoadService(HttpClient client)
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
            return RestService.For<IDataLoadService>(client, networkServiceSettings);
        }
    }
}
