using System.Collections.Generic;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Sprungbrett
{

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