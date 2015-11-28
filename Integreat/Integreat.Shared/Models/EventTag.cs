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

        [JsonProperty("id")]
        public int Id { get; set; }

        public EventTag() { }
        public EventTag(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

