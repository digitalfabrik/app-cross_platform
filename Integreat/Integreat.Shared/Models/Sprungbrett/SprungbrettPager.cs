using Newtonsoft.Json;

namespace Integreat.Shared.Models.Sprungbrett
{

        public class SprungbrettPager
        {
            [JsonProperty("current")]
            public int Current { get; set; }
            [JsonProperty("max")]
            public int Max { get; set; }
        }
    
}