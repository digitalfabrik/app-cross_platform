using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.Main;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class MainContentPageViewModel : BaseViewModel {
        #region Fields

        private INavigator _navigator;
        private Func<Language, Location, PageLoader> _pageLoaderFactory; // factory which creates a PageLoader for a given language and location

        private Location _lastLoadedLocation; // the last loaded location
        private Language _lastLoadedLanguage; // the last loaded language
        private PersistenceService _persistenceService; // persistence service for online or offline loading of data
        private Func<Page, PageViewModel> _pageViewModelFactory; // creates PageViewModel's out of Pages
        private IList<PageViewModel> _loadedPages;

        private Command _itemTappedCommand;
        private readonly Func<PageViewModel, IList<PageViewModel>, MainContentPageViewModel, MainTwoLevelViewModel> _twoLevelViewModelFactory; // factory which creates ViewModels for the two level view;
        private readonly Func<PageViewModel, MainContentPageViewModel, MainSingleItemDetailViewModel> _singleItemDetailViewModelFactory; // factory which creates ViewModels for the SingleItem view

        #endregion

        #region Properties

        public IList<PageViewModel> LoadedPages {
            get { return _loadedPages; }
            set { SetProperty(ref _loadedPages, value); }
        }

        
        public Command ItemTappedCommand {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }
        #endregion

        public MainContentPageViewModel(IAnalyticsService analytics, INavigator navigator, Func<Language, Location, PageLoader> pageLoaderFactory, PersistenceService persistenceService,
            Func<Page, PageViewModel> pageViewModelFactory
            , Func<PageViewModel, IList<PageViewModel>, MainContentPageViewModel, MainTwoLevelViewModel> twoLevelViewModelFactory
            , Func<PageViewModel, MainContentPageViewModel, MainSingleItemDetailViewModel> singleItemDetailViewModelFactory)
        : base(analytics) {
            Title = "Main content";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _pageLoaderFactory = pageLoaderFactory;
            _persistenceService = persistenceService;
            _pageViewModelFactory = pageViewModelFactory;
            _twoLevelViewModelFactory = twoLevelViewModelFactory;
            _singleItemDetailViewModelFactory = singleItemDetailViewModelFactory;

            ItemTappedCommand = new Command(OnTap);

            LoadSettings();
        }

        /// <summary>
        /// Called when the user [tap]'s on a item.
        /// </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        private async void OnTap(object pageViewModel)
        {
            var pageVm = pageViewModel as PageViewModel;
            if (pageVm == null) return;
            if (pageVm.Children.Count == 0)
            {
                // target page has no children, display only content
                await _navigator.PushAsync(_singleItemDetailViewModelFactory(pageVm, this));
            }
            else
            {
                // target page has children, display another two level view
                await _navigator.PushAsync(_twoLevelViewModelFactory(pageVm, LoadedPages, this));
            }
            
           /* var subpages = LoadedPages.Where(x => pageVm != null && x.Page.ParentId == pageVm.Page.PrimaryKey).ToList();
            if (subpages.Count > 0) {
                await _navigator.PushAsync(_detailedPagesViewModelFactory(pageVm, subpages));
            } else {
                pageVm?.ShowPageCommand.Execute(null);
            }*/
        }

        /// <summary>
        /// Loads the location and language from the settings and finally loads their models from the persistence service.
        /// </summary>
        private async void LoadSettings() {
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            IsBusy = true;
            _lastLoadedLanguage = await _persistenceService.Get<Language>(languageId);
            _lastLoadedLocation = await _persistenceService.Get<Location>(locationId);
            IsBusy = false;
        }

        protected override async void OnRefresh() {
            // wait until we're not busy anymore
            await Task.Run(() => {
                while (IsBusy) ;
            });
            LoadPages();
            await Task.Run(() => {
                while (IsBusy) ;
            });
        }

        /// <summary>
        /// Loads all pages for the given language and location from the persistenceService.
        /// </summary>
        /// <param name="forceRefresh">If set to <c>true</c> a [refresh is forced]. Indicating that it'll be loaded from the server.</param>
        /// <param name="forLocation">The selected location.</param>
        /// <param name="forLanguage">The selected language.</param>
        private async void LoadPages(bool forceRefresh = false, Location forLocation = null, Language forLanguage = null) {
            if (forLocation == null) forLocation = _lastLoadedLocation;
            if (forLanguage == null) forLanguage = _lastLoadedLanguage;

            if (forLanguage == null || forLocation == null || IsBusy) {
                Console.WriteLine("LoadPages could not be executed");
                return;
            }

            var pageLoader = _pageLoaderFactory(forLanguage, forLocation);
            Console.WriteLine("LoadPages called");
            try {
                IsBusy = true;
                //var parentPageId = _selectedPage?.Page?.PrimaryKey ?? Models.Page.GenerateKey(0, Location, Language);
                var pages = await pageLoader.Load(forceRefresh);

                LoadedPages = pages.Select(page => _pageViewModelFactory(page)).ToList();
                // set children
                SetChildrenProperties(LoadedPages);

                /* foreach (var pageViewModel in LoadedPages) {
                     pageViewModel.ChangeLocalLanguageCommand = ChangeLocalLanguageCommand;
                 }*/
            } finally {
                IsBusy = false;
                /* if (PageIdToShowAfterLoading != null) {


                     // find page id
                     var page = LoadedPages.FirstOrDefault(x => x.Page.PrimaryKey == PageIdToShowAfterLoading);
                     PageIdToShowAfterLoading = null;
                     if (page != null) {
                         // get the parent of the page we want to show
                         var parent = LoadedPages.FirstOrDefault(x => x.Page.PrimaryKey == page.Page.ParentId);
                         // emulate a tap on the parent (so the list gets pushed)
                         if (parent != null) OnTap(parent);
                         // then push the page itself
                         await _navigator.PushAsync(page);
                     }
                 }*/
            }
        }

        private void SetChildrenProperties(IList<PageViewModel> onPages)
        {
            // go through each page and set the children list
            foreach (var pageViewModel in onPages)
            {
                pageViewModel.Children = onPages.Where(x => x.Page.ParentId == pageViewModel.Page.PrimaryKey).ToList();
            }
        }
    }
}
