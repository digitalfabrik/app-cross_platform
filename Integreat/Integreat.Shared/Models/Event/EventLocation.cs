using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Location for a Event.
    /// </summary>
	public class EventLocation
	{
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
	}
}

