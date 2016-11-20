
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Integreat.Shared.Models
{
	[Table ("AvailableLanguage")]
	public class AvailableLanguage
	{
	    private Page _otherPage;
	    private string _otherPageId;

	    [PrimaryKey, AutoIncrement]
		public int PrimaryKey { get; set; }
        
		public string LanguageId{ get; set; }
        
        [ForeignKey(typeof(Page))]
        public string OwnPageId { get; set; }

        [ForeignKey(typeof(Page))]
	    public string OtherPageId {
	        get { return _otherPageId; }
	        set { _otherPageId = value; }
	    }

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

