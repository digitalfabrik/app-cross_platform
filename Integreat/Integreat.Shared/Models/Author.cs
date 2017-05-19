using Newtonsoft.Json;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Author of a page.
    /// </summary>
	public class Author
	{
		[JsonProperty ("login")]
		public string Login{ get; set; }

		[JsonProperty ("first_name")]
		public string FirstName{ get; set; }

		[JsonProperty ("last_name")]
		public string LastName{ get; set; }
	}
}

