using System;
using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("EventCategory")]
	public class EventCategory
	{  
		[PrimaryKey, Column("_id")]
        [JsonProperty("id")]
        public int Id	{get;set; }

        [JsonProperty("name")]
        public string Name{get;set; }

        [JsonProperty("parent")]
        public int Parent{get;set;}

		public int EventId{get;set;}
		public int PageId{get;set;}

        public EventCategory() { }
		public EventCategory(int id,string name, int parent) {
			Id = id;
			Name = name;
			Parent = parent;
		}
	}
}

