using System.Security;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign.Main
{
    [SecurityCritical]
    public partial class MainSingleItemDetailPage
    {
        public Command OnNavigatingCommand { get; set; }
        public MainSingleItemDetailPage ()
		{
			InitializeComponent ();
		}

	    private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
	    {
	        OnNavigatingCommand?.Execute(e);
        }
	}
}
