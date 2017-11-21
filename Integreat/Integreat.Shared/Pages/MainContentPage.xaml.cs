using System.Security;

namespace Integreat.Shared.Pages
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
