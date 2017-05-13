using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// todo description
    /// </summary>
	public class EventTag
	{
		[JsonProperty ("name")]
		public string Name { get; set; }

		[JsonProperty ("id")]
    	public int Id { get; set; }
	}
}

