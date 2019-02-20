using System.Security;

namespace App1.Pages
{
    /// <summary>
    /// Page implemetation for  Extras
    /// </summary>
    public partial class ExtrasContentPage
	{
	    [SecurityCritical]
        public ExtrasContentPage ()
		{
			InitializeComponent ();
		}
	}
}
