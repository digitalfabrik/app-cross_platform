
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Models
{
	[Table("AvailableLanguage")]
	public class AvailableLanguage 
    {
        [PrimaryKey, AutoIncrement]
        public int PrimaryKey { get; set; }
        public string Language{ get; set; }
        public int OtherPageId{ get; set; }

        [ForeignKey(typeof(Page))]
        public int OwnPageId { get; set; }

		public AvailableLanguage (string language, int otherPageId)
		{
			Language = language;
			OtherPageId = otherPageId;
        }
        public AvailableLanguage() { }
    }
}

