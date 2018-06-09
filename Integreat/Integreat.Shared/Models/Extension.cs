using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    public class Extension
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
