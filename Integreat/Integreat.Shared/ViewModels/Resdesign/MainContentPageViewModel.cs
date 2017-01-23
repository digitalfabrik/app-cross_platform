using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Pages.Redesign;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.Main;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class MainContentPageViewModel : BaseContentViewModel {
        #region Fields

        private INavigator _navigator;
        private Func<Language, Location, PageLoader> _pageLoaderFactory; // factory which creates a PageLoader for a given language and location
        
        private Func<Page, PageViewModel> _pageViewModelFactory; // creates PageViewModel's out of Pages
        private IList<PageViewModel> _loadedPages;

        private Command _itemTappedCommand;
        private readonly Func<PageViewModel, IList<PageViewModel>, MainTwoLevelViewModel> _twoLevelViewModelFactory; // factory which creates ViewModels for the two level view;
        private readonly Func<PageViewModel, MainSingleItemDetailViewModel> _singleItemDetailViewModelFactory; // factory which creates ViewModels for the SingleItem view
        private IList<PageViewModel> _rootPages;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the loaded pages. (I.e. all pages for the selected region/language)
        /// </summary>
        /// <value>
        /// The loaded pages.
        /// </value>
        private IList<PageViewModel> LoadedPages {
            get { return _loadedPages; }
            set { SetProperty(ref _loadedPages, value); }
        }

        /// <summary>
        /// Gets or sets the root pages. That are all pages without parents.
        /// </summary>
        /// <value>
        /// The root pages.
        /// </value>
        public IList<PageViewModel> RootPages
        {
            get { return _rootPages; }
            set { SetProperty(ref _rootPages, value); }
        }


        public Command ItemTappedCommand {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }
        #endregion

        public MainContentPageViewModel(IAnalyticsService analytics, INavigator navigator, Func<Language, Location, PageLoader> pageLoaderFactory, PersistenceService persistenceService,
            Func<Page, PageViewModel> pageViewModelFactory
            , Func<PageViewModel, IList<PageViewModel>, MainTwoLevelViewModel> twoLevelViewModelFactory
            , Func<PageViewModel, MainSingleItemDetailViewModel> singleItemDetailViewModelFactory)
        : base(analytics, persistenceService) {
            Title = "Main content";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _pageLoaderFactory = pageLoaderFactory;
            _pageViewModelFactory = pageViewModelFactory;
            _twoLevelViewModelFactory = twoLevelViewModelFactory;
            _singleItemDetailViewModelFactory = singleItemDetailViewModelFactory;

            ItemTappedCommand = new Command(OnPageTapped);
        }

        /// <summary>
        /// Called when the user [tap]'s on a item.
        /// </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        private async void OnPageTapped(object pageViewModel)
        {
            var pageVm = pageViewModel as PageViewModel;
            if (pageVm == null) return;
            if (pageVm.Children.Count == 0)
            {
                // target page has no children, display only content
                await _navigator.PushAsync(_singleItemDetailViewModelFactory(pageVm), Navigation);
            }
            else
            {
                // target page has children, display another two level view
                await _navigator.PushAsync(_twoLevelViewModelFactory(pageVm, LoadedPages), Navigation);
            }
            
           /* var subpages = LoadedPages.Where(x => pageVm != null && x.Page.ParentId == pageVm.Page.PrimaryKey).ToList();
            if (subpages.Count > 0) {
                await _navigator.PushAsync(_detailedPagesViewModelFactory(pageVm, subpages));
            } else {
                pageVm?.OnTapCommand.Execute(null);
            }*/
        }




        /// <summary>
        /// Loads all pages for the given language and location from the persistenceService.
        /// </summary>
        protected override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null) {
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            if (IsBusy || forLocation == null || forLanguage == null) {
                Console.WriteLine("LoadPages could not be executed");
                return;
            }

            var pageLoader = _pageLoaderFactory(forLanguage, forLocation);
            Console.WriteLine("LoadPages called");
            try {
                IsBusy = true;
                LoadedPages?.Clear();
                //var parentPageId = _selectedPage?.Page?.PrimaryKey ?? Models.Page.GenerateKey(0, Location, Language);
                var pages = await pageLoader.Load(forced);

                LoadedPages = pages.Select(page => _pageViewModelFactory(page)).ToList();

                // register commands
                foreach (var pageViewModel in LoadedPages)
                {
                    pageViewModel.OnTapCommand = new Command(OnPageTapped);
                }

                // set children
                SetChildrenProperties(LoadedPages);

                SetRootPages();

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

        /// <summary>
        /// Sets the root pages.
        /// </summary>
        private void SetRootPages() {
            //var id = SelectedPage?.Page?.PrimaryKey ?? "0";
            var key = Models.Page.GenerateKey("0", LastLoadedLocation, LastLoadedLanguage);
            RootPages = LoadedPages.Where(x => x.Page.ParentId == key).OrderBy(x => x.Page.Order).ToList();
        }

        /// <summary>
        /// Sets the children properties for each given page.
        /// </summary>
        /// <param name="onPages">The target pages.</param>
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
