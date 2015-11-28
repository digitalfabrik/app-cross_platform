using System;
using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("EventLocation")]
	public class EventLocation
    {
        [JsonProperty("name")]
        public string Name{get;set; }

        [JsonProperty("address")]
        public string Address{get;set; }

        [JsonProperty("town")]
        public string Town{get;set; }

        [JsonProperty("state")]
        public string State{get;set; }

        [JsonProperty("region")]
        public string Region{get;set; }

        [JsonProperty("country")]
        public string Country{get;set; }

        [JsonProperty("latitude")]
        public double Latitude{get;set; }

        [JsonProperty("longitude")]
        public double Longitude {get;set; }

        [JsonProperty("postcode")]
        public int Postcode{get;set; }

        [JsonProperty("id")]
        [PrimaryKey, Column("_id")]
		public int Id{get;set;}

        public EventLocation() { }
        public EventLocation(int id,  string name,  string address,  string town,  string state, int postcode,  string region,  string country, double latitude, double longitude) {
			Id = id;
			Name = name;
			Address = address;
			Town = town;
			State = state;
			Postcode = postcode;
			Region = region;
			Country = country;
			Latitude = latitude;
			Longitude = longitude;
		}
	}
}

