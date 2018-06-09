using System;
using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    public class Extra
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}
