using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign.Main
{
	public partial class MainSingleItemDetailPage : BaseContentPage
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
