using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
	{
		public IEnumerable<Page> LoadedPages { get; set; }

		public ObservableCollection<Page> VisiblePages { get; set; }

		private int _selectedPagePrimaryKey = -1;
		public PageLoader PageLoader;

		public int SelectedPagePrimaryKey {
			get { return _selectedPagePrimaryKey; }
			set {
				_selectedPagePrimaryKey = value;
				FilterPages ();
			}
		}

		public PagesViewModel ()
		{
			Title = "Information";
			Icon = null;
			VisiblePages = new ObservableCollection<Page> ();
			SelectedPagePrimaryKey = -1;
		}

		private Command _loadPagesCommand;

		public Command LoadPagesCommand {
			get {
				Console.WriteLine ("LoadPagesCommand called");
				return _loadPagesCommand;
			}
			set { SetProperty (ref _loadPagesCommand, value); }
		}

		public void PagesLoaded (IEnumerable<Page> pages)
		{
			Console.WriteLine ("PagesLoaded in PagesViewModel called");
			IsBusy = false;
			LoadedPages = pages;
			FilterPages ();
		}

		private void FilterPages ()
		{
			if (LoadedPages == null) {
				return;
			}
			var filteredPages = LoadedPages
                .Where (x => _selectedPagePrimaryKey == -1) //todo comment this in: || x.ParentId == _selectedPagePrimaryKey)
                .OrderBy (x => x.Order);
			VisiblePages.Clear ();
			VisiblePages.AddRange (filteredPages);
		}
	}
}
