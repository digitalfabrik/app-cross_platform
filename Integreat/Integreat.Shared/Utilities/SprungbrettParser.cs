using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Integreat.Shared.Utilities
{
    public class SprungbrettTemp
    {
        public async Task<RootObject> FetchJobOffersAsync(string url)
        {
            var client = new WebClient();


            var wc = new WebClient { Proxy = null };
            var json = wc.DownloadString(new Uri(url));
            var rootJson = JsonConvert.DeserializeObject<RootObject>(json);

            return rootJson;
        }

        public class RootObject
        {
            [JsonProperty("total")]
            public string Total { get; set; }
            [JsonProperty("pager")]
            public Pager Pager { get; set; }
            [JsonProperty("results")]
            public List<JobOffer> JobOffers { get; set; }
        }
        //JobOffer class to save and manipulate the json elements
        public class JobOffer
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("apprenticeship")]
            public string Apprenticeship { get; set; }
            [JsonProperty("employment")]
            public string Employment { get; set; }
            [JsonProperty("company")]
            public string Company { get; set; }
            [JsonProperty("street")]
            public string Street { get; set; }
            [JsonProperty("zip")]
            public string Zip { get; set; }
            [JsonProperty("city")]
            public string City { get; set; }
            [JsonProperty("lat")]
            public string Lat { get; set; }
            [JsonProperty("lon")]
            public string Lon { get; set; }
            [JsonProperty("schooltypegroup")]
            public string Schooltypegroup { get; set; }
            [JsonProperty("distance")]
            public string Distance { get; set; }
            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Pager
        {
            [JsonProperty("current")]
            public int Current { get; set; }
            [JsonProperty("max")]
            public int Max { get; set; }
        }
    }
}