using SQLite.Net.Attributes;

namespace Integreat
{
	[Table("Author")]
	public class Author
	{
		[PrimaryKey, Column("_id")]
		public string Login{ get; set; }
		public string FirstName{ get; set; }
		public string LastName{ get; set; }

		public Author (string login, string firstName, string lastName)
		{
			Login = login;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}

