using System.Security;
using Xamarin.Forms.Xaml;

namespace Integreat.Shared.Pages.Redesign.General
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[SecurityCritical]
    public partial class GeneralWebViewPage
	{
	    [SecurityCritical]
        public GeneralWebViewPage ()
		{
			InitializeComponent ();
		}
	}
}
