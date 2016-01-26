using System.Collections.Generic;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.Pages
{
	public partial class OverviewPage : TabbedPage
	{

		public void PagesLoaded (IEnumerable<Page> pages)
		{
			IsBusy = false;
			InformationOverview.ViewModel.PagesLoaded (pages);
		}

		public void PageSelected (int index)
		{
			InformationOverview.ViewModel.SelectedPagePrimaryKey = index;
		}

		public OverviewPage (Command loadPagesCommand)
		{
			InitializeComponent ();
			InformationOverview.ViewModel.LoadPagesCommand = loadPagesCommand;
		}
        
	}
}
