using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
    {
        public ObservableCollection<PageViewModel> LoadedPages {get; set;}
        public ObservableCollection<PageViewModel> VisiblePages {get; set; }

        public PagesViewModel()
        {
            Title = "Information";
            LoadedPages = new ObservableCollection<PageViewModel>();
            VisiblePages = new ObservableCollection<PageViewModel>();
        }
        

        private int _selectedPagePrimaryKey = -1;
        public int SelectedPagePrimaryKey {
			get { return _selectedPagePrimaryKey; }
			set {
				_selectedPagePrimaryKey = value;
				FilterPages ();
			}
        }

        private void FilterPages()
        {
            //TODO change VisiblePages
        }

        public Command LoadPagesCommand { get; set; }
	}
}
