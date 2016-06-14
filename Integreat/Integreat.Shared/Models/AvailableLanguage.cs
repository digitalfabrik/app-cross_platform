
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("AvailableLanguage")]
	public class AvailableLanguage
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

		public string Language{ get; set; }

		public int OtherPageId{ get; set; }

		[ForeignKey (typeof(Page))]
		public string OwnPageId { get; set; }

		public AvailableLanguage (string language, int otherPageId)
		{
			Language = language;
			OtherPageId = otherPageId;
		}

		public AvailableLanguage ()
		{
		}
	}
}

