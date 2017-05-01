using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
	public class EventTag
	{

		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("id")]
		public int Id { get; set; }

	}
}

