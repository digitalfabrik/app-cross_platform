using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	public class Location
	{
		[PrimaryKey]
		[JsonProperty ("id")]
		public int Id{ get; set; }

		[JsonProperty ("live")]
		public bool Live { get; set; }

		public DateTime Modified { get; set; }
		//TODO

		[JsonProperty ("name")]
		public string Name{ get; set; }

		[JsonProperty ("icon")]
		public string Icon{ get; set; }

		[JsonProperty ("path")]
		public string Path{ get; set; }

		[JsonProperty ("description")]
		public string Description{ get; set; }

		[JsonProperty ("color")]
		public string Color{ get; set; }

		[JsonProperty ("cover_image")]
		public string CityImage{ get; set; }

		[JsonProperty ("latitude")]
		public float Latitude{ get; set; }

		[JsonProperty ("longitude")]
		public float Longitude{ get; set; }

		[OneToMany (CascadeOperations = CascadeOperation.All)]
		public List<Language> Languages { get; set; }

        /// <summary>
        /// Gets the key to group locations, which is just the first letter of the name (uppercase).
        /// </summary>
        public string GroupKey => Name?.ElementAt(0).ToString().ToUpper();

		public Location ()
		{
		}

		public Location (int id, string name, string icon, string path, 
		                 string description, string color, string cityImage, 
		                 float latitude, float longitude, 
		                 bool live)
		{
			Id = id;
			Name = name;
			Icon = icon;
			Path = path;
			Description = description;
			Color = color;
			CityImage = cityImage;
			Latitude = latitude;
			Longitude = longitude;
            Live = live;
		}

		public override string ToString ()
		{
            var tmp = Path.Replace("/wordpress/", "");

            return tmp.Substring (0, tmp.Length - 1);
		}

	    public bool Find(string searchText)
	    {
	        if (!Live)
	        {
	            return "wirschaffendas".Equals(searchText);
	        }
            var locationString = (Description ?? "") + (Name ?? "");
            return locationString.ToLower().Contains((searchText ?? "").ToLower());
	    }
	}
}

