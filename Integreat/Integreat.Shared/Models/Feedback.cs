using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    public class Feedback
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }
    }
}
