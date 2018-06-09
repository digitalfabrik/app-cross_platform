using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models.Extras
{
    public class Extra
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
