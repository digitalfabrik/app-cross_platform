using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;
using Integreat.Shared.Pages;

namespace Integreat.Shared.ViewModels
{
	public class PagesViewModel : BaseViewModel
	{
		public IEnumerable<Models.Page> LoadedPages { get; set; }

		public ObservableCollection<Models.Page> VisiblePages { get; set; }

		private int _selectedPagePrimaryKey = -1;
		public PageLoader PageLoader;
        private Page page;

        private INavigation navigation;

        public int SelectedPagePrimaryKey {
			get { return _selectedPagePrimaryKey; }
			set {
				_selectedPagePrimaryKey = value;
				FilterPages ();
			}
        }
        
        public PagesViewModel (INavigation navigation, Page page) //TODO page should not be included, but currently needed for dialog
		{
			Title = "Information";
			Icon = null;
			VisiblePages = new ObservableCollection<Models.Page> ();
			SelectedPagePrimaryKey = -1;
            if (navigation == null)
            {
                throw new ArgumentNullException("navigation");
            }
            this.navigation = navigation;

            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            this.page = page;
        }

		private Command _loadPagesCommand;

		public Command LoadPagesCommand {
			get {
				Console.WriteLine ("LoadPagesCommand called");
				return _loadPagesCommand;
			}
			set { SetProperty (ref _loadPagesCommand, value); }
		}

        private Command _openSearchCommand;

        public Command OpenSearchCommand
        {
            get
            {
                return _openSearchCommand ??
                (_openSearchCommand = new Command(() => {
                    onSearchClicked();
                }));
            }
        }

        private Command _changeLanguageCommand;

        public Command ChangeLanguageCommand
        {
            get
            {
                return _changeLanguageCommand ??
                (_changeLanguageCommand = new Command(() => {
                    onChangeLanguageClicked();
                }));
            }
        }

        private async void onChangeLanguageClicked()
        {
            var action = await page.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        }

        void onSearchClicked()
        {
            var search = new PageSearchList(this);
            navigation.PushAsync(search);
        }

        public void PagesLoaded (IEnumerable<Models.Page> pages)
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
