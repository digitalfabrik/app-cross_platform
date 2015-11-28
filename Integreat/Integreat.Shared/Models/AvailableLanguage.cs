
using SQLite.Net.Attributes;

namespace Integreat.Models
{
	[Table("AvailableLanguage")]
	public class AvailableLanguage
	{
        public string Language{ get; set; }
        public int OtherPageId{ get; set; }
		public int OwnPageId{ get; set; }

		public AvailableLanguage (string language, int otherPageId)
		{
			Language = language;
			OtherPageId = otherPageId;
        }
        public AvailableLanguage() { }
    }
}

