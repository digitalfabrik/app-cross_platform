using System.Security;

namespace App1.Pages
{
    /// <summary>
    /// This is the MainPage with category tiles
    /// </summary>
    [SecurityCritical]
    public partial class MainContentPage
    {
        [SecurityCritical]
        public MainContentPage ()
		{
			InitializeComponent ();
		}
	}
}
