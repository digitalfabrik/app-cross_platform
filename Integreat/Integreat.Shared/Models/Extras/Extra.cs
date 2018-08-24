using System.Collections.Generic;
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

        [JsonProperty("post")]
        public IDictionary<string, string> Post { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        public string Title => Name;

        public override string ToString()
        {
            return Name;
        }
    }
}
