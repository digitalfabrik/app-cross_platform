using System;
using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;

namespace Integreat
{
    [Table("EventTag")]
    public class EventTag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public int EventId { get; set; }

        public EventTag(string name)
        {
            Name = name;
        }
    }
}

