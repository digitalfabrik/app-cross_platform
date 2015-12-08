using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
	public partial class OverviewPage : TabbedPage
    {
        public OverviewPage(int pagePrimaryKey)
        {
            InitializeComponent();
            InformationOverview.ViewModel.SelectedPagePrimaryKey = pagePrimaryKey;
        }
        
	}
}
