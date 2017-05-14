using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Describes a Tag for an Event.
    /// </summary>
	public class EventTag
	{
		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("id")]
    	public int Id { get; set; }
	}
}

