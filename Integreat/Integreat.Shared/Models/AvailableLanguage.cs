
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("AvailableLanguage")]
	public class AvailableLanguage
	{
		[PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }

        [ManyToOne(foreignKey: "LanguageId")]
	    public Language Language { get; set; }

        [ForeignKey(typeof(Language))]
		public string LanguageId{ get; set; }

        [ForeignKey(typeof(Page))]
        public string OtherPageId{ get; set; }

        public string OwnPageId { get; set; }

        [ManyToOne(foreignKey: "OtherPageId")]
        public Page OtherPage { get; set; }

	    public AvailableLanguage (string languageId, string otherPageId)
		{
			LanguageId = languageId;
			OtherPageId = otherPageId;
		}

		public AvailableLanguage ()
		{
		}
	}
}

