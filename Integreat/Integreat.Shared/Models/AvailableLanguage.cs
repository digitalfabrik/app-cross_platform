

namespace Integreat.Shared.Models
{

	public class AvailableLanguage
	{
	    private Page _otherPage;
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

