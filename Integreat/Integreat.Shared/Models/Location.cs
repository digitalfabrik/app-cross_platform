using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("Location")]
	public class Location
	{   
		[PrimaryKey, Column("_id")]
        [JsonProperty("id")]
        public int Id{get;set; }

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

