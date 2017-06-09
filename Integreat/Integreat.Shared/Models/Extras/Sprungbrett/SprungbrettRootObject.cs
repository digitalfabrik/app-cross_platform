using System.Collections.Generic;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Extras.Sprungbrett
{
    /// <summary>
    /// the root object from recived json 
    /// Example for Augsburg:
    /// https://www.sprungbrett-intowork.de/ajax/app-search-internships?location=augsburg
    /// </summary>
    public class SprungbrettRootObject
    {
        [JsonProperty("total")]
        public string Total { get; set; }
        [JsonProperty("pager")]
        public SprungbrettPager Pager { get; set; }
        [JsonProperty("results")]
        public List<SprungbrettJobOffer> JobOffers { get; set; }
    }
}