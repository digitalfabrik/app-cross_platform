using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// todo description
    /// </summary>
	public class EventCategory
	{
		[JsonProperty ("id")]
		public int Id	{ get; set; }

		[JsonProperty ("name")]
		public string Name{ get; set; }
	}
}

