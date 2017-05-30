

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Model for the list of availableLanguages in a page. Note that this is not automatically parsed, but rather manually in Page.cs
    /// </summary>
	public class AvailableLanguage
	{
	    //private Page _otherPage;
	    private string _otherPageId;

        
		public string LanguageId{ get; set; }
        

        public string OwnPageId { get; set; }

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

