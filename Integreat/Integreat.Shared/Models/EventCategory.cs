using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
	public class EventCategory
	{

		[JsonProperty ("id")]
		public int Id	{ get; set; }

		[JsonProperty ("name")]
		public string Name{ get; set; }

	}
}

