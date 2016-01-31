using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
	{
	    private ObservableCollection<PageViewModel> _loadedPages; 
	    public ObservableCollection<PageViewModel> LoadedPages
	    {
	        get { return _loadedPages; }
	        set
	        {
	            _loadedPages = value;
                FilterPages();
	        }
	    }

	    private ObservableCollection<PageViewModel> _visiblePages;

	    public ObservableCollection<PageViewModel> VisiblePages
	    {
	        get { return _visiblePages; }
	        set
	        {
	            _visiblePages = value;
	            OnPropertyChanged();
	        }
	    }

	    private PageViewModel _selectedPage;
        public PageViewModel SelectedPage { get { return _selectedPage; }
            set
            {
                _selectedPage = value;
                FilterPages();
            } }

        public PagesViewModel()
        {
            Title = "Information";
            LoadedPages = new ObservableCollection<PageViewModel>();
            VisiblePages = new ObservableCollection<PageViewModel>();
        }
        
        private void FilterPages()
        {
            VisiblePages = new ObservableCollection<PageViewModel>(LoadedPages.Where(x=> SelectedPage == null || SelectedPage.Page.Id == x.Page.ParentId).OrderBy(x => x.Page.Order));
        }

        public Command LoadPagesCommand { get; set; }
    }
}
