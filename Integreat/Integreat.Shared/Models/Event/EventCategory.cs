using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Category for a event.
    /// </summary>
	public class EventCategory
	{
		[JsonProperty ("id")]
		public int Id	{ get; set; }

		[JsonProperty ("name")]
		public string Name{ get; set; }
	}
}

