using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Models
{
	public class Location
    {   
        [PrimaryKey]
        [JsonProperty("id")]
        public int Id{get;set; }

	    public DateTime Modified { get; set; } //TODO

        [JsonProperty("name")]
        public string Name{get;set; }

        [JsonProperty("icon")]
        public string Icon{get;set; }

        [JsonProperty("path")]
        public string Path{get;set; }

        [JsonProperty("description")]
        public string Description{get;set; }

        [JsonProperty("global")]
        public bool Global{get;set; }

        [JsonProperty("color")]
        public string Color{get;set; }

        [JsonProperty("cover_image")]
        public string CityImage{get;set; }

        [JsonProperty("latitude")]
        public float Latitude{get;set; }

        [JsonProperty("longitude")]
        public float Longitude{get;set;}

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Language> Languages { get; set; }

        public Location() { }
		public Location(int id, string name, string icon, string path, string description, bool global, string color, string cityImage, float latitude, float longitude) {
			Id = id;
			Name = name;
			Icon = icon;
			Path = path;
			Description = description;
			Global = global;
			Color = color;
			CityImage = cityImage;
			Latitude = latitude;
			Longitude = longitude;
		}
       
        public override string ToString()
        {
            return Path.Substring(1, Path.Length - 1);
        }
    }
}

