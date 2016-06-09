using System;
using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("EventLocation")]
	public class EventLocation
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

		[ForeignKey (typeof(EventPage))]
		public string PageId { get; set; }

		[JsonProperty ("name")]
		public string Name{ get; set; }

		[JsonProperty ("address")]
		public string Address{ get; set; }

		[JsonProperty ("town")]
		public string Town{ get; set; }

		[JsonProperty ("state")]
		public string State{ get; set; }

		[JsonProperty ("region")]
		public string Region{ get; set; }

		[JsonProperty ("country")]
		public string Country{ get; set; }

		[JsonProperty ("latitude")]
		public double Latitude{ get; set; }

		[JsonProperty ("longitude")]
		public double Longitude { get; set; }

		[JsonProperty ("postcode")]
		public int Postcode{ get; set; }

		[JsonProperty ("id")]
		public int Id{ get; set; }

		public EventLocation ()
		{
		}

		public EventLocation (int id, string name, string address, string town, string state, int postcode, string region, string country, double latitude, double longitude)
		{
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

