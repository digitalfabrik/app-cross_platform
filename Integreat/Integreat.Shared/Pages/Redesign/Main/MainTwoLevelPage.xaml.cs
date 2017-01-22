using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign.Main
{
	public partial class MainTwoLevelPage : BaseContentPage {
		public MainTwoLevelPage ()
		{
			InitializeComponent ();
		}


	    private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
	    {
            // TODO: Use attached behavior instead of this code-behind approach to bind the ItemTapped event to the command
	        var page = (sender as ListView)?.SelectedItem as PageViewModel;
	        page?.OnTapCommand.Execute(page);
	    }
	}
}
