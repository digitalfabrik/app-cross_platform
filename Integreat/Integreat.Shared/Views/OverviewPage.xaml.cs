using System.Collections.Generic;
using Xamarin.Forms;
using Page = Integreat.Models.Page;

namespace Integreat.Shared.Views
{
	public partial class OverviewPage : TabbedPage
    {
        public OverviewPage()
        {
            InitializeComponent();
        }

	    public void SetPages(IEnumerable<Page> childrenPages)
        {
           informationOverview.SetPages(childrenPages);
        }
	}
}
