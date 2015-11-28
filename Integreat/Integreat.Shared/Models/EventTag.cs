using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat.Models
{
    [Table("EventTag")]
    public class EventTag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public int EventId { get; set; }

        public EventTag() { }
        public EventTag(string name)
        {
            Name = name;
        }
    }
}

