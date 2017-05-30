using System;
using System.Security;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign
{
    [SecurityCritical]
    public partial class SettingsContentPage
	{

	    public Command OpenLanguageSelectionCommand { get; set; }

	    public Command OpenLocationSelectionCommand { get; set; }

		public SettingsContentPage ()
		{
			InitializeComponent ();
		}

	    private void LocationView_OnTapped(object sender, EventArgs e)
	    {
	        OpenLocationSelectionCommand?.Execute(this);
	    }

	    private void LanguageView_OnTapped(object sender, EventArgs e)
	    {
            OpenLanguageSelectionCommand?.Execute(this);

        }
	}
}
