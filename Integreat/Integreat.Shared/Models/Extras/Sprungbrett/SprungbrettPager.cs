using Newtonsoft.Json;

namespace Integreat.Shared.Models.Extras.Sprungbrett
{
    /// <summary>
    /// additional pager information parsed from json
    /// </summary>
    public class SprungbrettPager
    {
        [JsonProperty("current")]
        public int Current { get; set; }
        [JsonProperty("max")]
        public int Max { get; set; }
    }
}