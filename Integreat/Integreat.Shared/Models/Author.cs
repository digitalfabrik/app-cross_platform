using Newtonsoft.Json;
using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("Author")]
	public class Author
	{
		[PrimaryKey, Column("_id")]
        [JsonProperty("login")]
        public string Login{ get; set; }

        [JsonProperty("first_name")]
        public string FirstName{ get; set; }

        [JsonProperty("last_name")]
        public string LastName{ get; set; }

		public Author (string login, string firstName, string lastName)
		{
			Login = login;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}

