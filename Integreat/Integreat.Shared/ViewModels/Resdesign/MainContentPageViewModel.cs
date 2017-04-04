﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.Main;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;
using Localization;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class MainContentPageViewModel : BaseContentViewModel {
        #region Fields

        private INavigator _navigator;

        private Func<Page, PageViewModel> _pageViewModelFactory; // creates PageViewModel's out of Pages
        private IList<PageViewModel> _loadedPages;

        private Command _itemTappedCommand;
        private readonly Func<PageViewModel, IList<PageViewModel>, MainTwoLevelViewModel> _twoLevelViewModelFactory; // factory which creates ViewModels for the two level view;
        private readonly Func<PageViewModel, MainSingleItemDetailViewModel> _singleItemDetailViewModelFactory; // factory which creates ViewModels for the SingleItem view
        private readonly Func<IEnumerable<PageViewModel>, SearchViewModel> _pageSearchViewModelFactory;
        private ObservableCollection<PageViewModel> _rootPages;
        private ICommand _changeLanguageCommand;
        private ICommand _openSearchCommand;
        private ICommand _openSettingsCommand;
        private IDialogProvider _dialogProvider;
        private ContentContainerViewModel _contentContainer;
        private Stack<PageViewModel> _shownPages;
        private string _pageIdToShowAfterLoading;
        private Func<SettingsContentPageViewModel> _settingsContentPageViewModelFactory;
        private DataLoaderProvider _dataLoaderProvider;

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
        public ObservableCollection<PageViewModel> RootPages {
            get { return _rootPages; }
            set { SetProperty(ref _rootPages, value); }
        }


        public Command ItemTappedCommand {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }

        public ICommand OpenSearchCommand {
            get { return _openSearchCommand; }
            set { SetProperty(ref _openSearchCommand, value); }
        }

        public ICommand ChangeLanguageCommand {
            get { return _changeLanguageCommand; }
            set { SetProperty(ref _changeLanguageCommand, value); }
        }

        public ICommand OpenSettingsCommand {
            get { return _openSettingsCommand; }
            set { SetProperty(ref _openSettingsCommand, value); }
        }

        public ContentContainerViewModel ContentContainer {
            get { return _contentContainer; }
            set { SetProperty(ref _contentContainer, value); }
        }
        private string RootParentId => Page.GenerateKey("0", LastLoadedLocation, LastLoadedLanguage);

        #endregion

        public MainContentPageViewModel(IAnalyticsService analytics, INavigator navigator, DataLoaderProvider dataLoaderProvider,
            Func<Page, PageViewModel> pageViewModelFactory
            , IDialogProvider dialogProvider
            , Func<PageViewModel, IList<PageViewModel>, MainTwoLevelViewModel> twoLevelViewModelFactory
            , Func<PageViewModel, MainSingleItemDetailViewModel> singleItemDetailViewModelFactory
            , Func<IEnumerable<PageViewModel>, SearchViewModel> pageSearchViewModelFactory
            , Func<SettingsContentPageViewModel> settingsContentPageViewModelFactory)
        : base(analytics, dataLoaderProvider) {

            Title = AppResources.Categories;
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _dataLoaderProvider = dataLoaderProvider;
            _pageViewModelFactory = pageViewModelFactory;
            _twoLevelViewModelFactory = twoLevelViewModelFactory;
            _singleItemDetailViewModelFactory = singleItemDetailViewModelFactory;
            _dialogProvider = dialogProvider;
            _pageSearchViewModelFactory = pageSearchViewModelFactory;
            _settingsContentPageViewModelFactory = settingsContentPageViewModelFactory;

            _shownPages = new Stack<PageViewModel>();
            ItemTappedCommand = new Command(OnPageTapped);
            OpenSearchCommand = new Command(OnOpenSearch);
            ChangeLanguageCommand = new Command(OnChangeLanguage);
            OpenSettingsCommand = new Command(OnOpenSettings);
        }

        private async void OnChangeLanguage(object obj) {
            if (IsBusy) return;

            // if there are no pages in the stack, it means we're in root. Show the normal language selection
            if (_shownPages.IsNullOrEmpty()) {
                ContentContainer.OpenLanguageSelection();
                return;
            }

            // get the current shown page
            var pageModel = _shownPages.Peek().Page;
            if (pageModel.AvailableLanguages.IsNullOrEmpty()) {
                return; // abort if there are no other languages available
            }

            // get the languages the page is available in. These only contain short names and ids (not keys), therefore we need to parse them a bit
            var languageShortNames = pageModel.AvailableLanguages.Select(x => x.LanguageId);

            // gets all available languages for the current location
            var languages = (await LoadLanguages()).ToList();
            // filter them by the available language short names
            var availableLanguages = languages.Where(x => languageShortNames.Contains(x.ShortName)).ToList();
            // get the full names for the short names
            var displayedNames = availableLanguages.Select(x => x.Name).ToArray();

            // display a selection popup and await the user interaction
            var action = await _dialogProvider.DisplayActionSheet("Select a Language?", "Cancel", null, displayedNames);

            // action contains the selected wording, or null if the user aborted. Get the selected language
            var selectedLanguage = availableLanguages.FirstOrDefault(x => x.Name == action);
            if (selectedLanguage != null) {
                // load and show page. Get the page Id and generate the page key
                var otherPageId = pageModel.AvailableLanguages.First(x => x.LanguageId == selectedLanguage.ShortName).OtherPageId;
                var otherPageKey = Page.GenerateKey(otherPageId, selectedLanguage.Location, selectedLanguage);

                _pageIdToShowAfterLoading = otherPageKey;

                await Navigation.PopToRootAsync();
                _shownPages.Clear();

                // set new language
                Preferences.SetLanguage(Preferences.Location(), selectedLanguage);
                ContentContainer.RefreshAll(true);
            } else {
                Debug.Write("No language selected");
            }
        }

        private async void OnOpenSearch(object obj) {
            if (IsBusy) return;

            await _navigator.PushAsync(_pageSearchViewModelFactory(LoadedPages), Navigation);
        }

        private async Task<IEnumerable<Language>> LoadLanguages()
        {
            return await _dataLoaderProvider.LanguagesDataLoader.Load(false, LastLoadedLocation ?? (LastLoadedLocation =
                    (await _dataLoaderProvider.LocationsDataLoader.Load(false)).FirstOrDefault(x => x.Id == Preferences.Location())));
        }

        /// <summary>
        /// Called when the user [tap]'s on a item.
        /// </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        private async void OnPageTapped(object pageViewModel) {
            var pageVm = pageViewModel as PageViewModel;
            if (pageVm == null) return;
            _shownPages.Push(pageVm);
            if (pageVm.Children.Count == 0) {
                // target page has no children, display only content
                await _navigator.PushAsync(_singleItemDetailViewModelFactory(pageVm), Navigation);
            } else {
                // target page has children, display another two level view
                await _navigator.PushAsync(_twoLevelViewModelFactory(pageVm, LoadedPages), Navigation);
            }
        }

        private async void OnOpenSettings(object obj) {
            if (IsBusy) return;
            SettingsContentPageViewModel settingsContentPageViewModel = _settingsContentPageViewModelFactory();
            settingsContentPageViewModel.RefreshCommand.Execute(false);
            await _navigator.PushModalAsync(settingsContentPageViewModel);

        }

        /// <summary>
        /// Loads all pages for the given language and location from the persistenceService.
        /// </summary>
        protected override async void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null) {
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;

            if (IsBusy || forLocation == null || forLanguage == null) {
                Debug.WriteLine("LoadPages could not be executed");
                if (IsBusy) Debug.WriteLine("The app is busy");
                if (forLocation == null) Debug.WriteLine("Location is null");
                if (forLanguage == null) Debug.WriteLine("Language is null");

                return;
            }

            try {
                IsBusy = true;
                LoadedPages?.Clear();
                RootPages?.Clear();
                //var parentPageId = _selectedPage?.Page?.PrimaryKey ?? Models.Page.GenerateKey(0, Location, Language);
                var pages = await _dataLoaderProvider.PagesDataLoader.Load(forced, forLanguage, forLocation);

                LoadedPages = pages.Select(page => _pageViewModelFactory(page)).ToList();

                // register commands
                foreach (var pageViewModel in LoadedPages) {
                    pageViewModel.OnTapCommand = new Command(OnPageTapped);
                }

                // set children
                SetChildrenProperties(LoadedPages);

                SetRootPages();

            } finally {
                if (_pageIdToShowAfterLoading != null && LoadedPages != null) {
                    // find page id
                    var page = LoadedPages.FirstOrDefault(x => x.Page.PrimaryKey == _pageIdToShowAfterLoading);
                    _pageIdToShowAfterLoading = null;


                    if (page != null) {
                        var pagesToPush = new List<PageViewModel> { page };
                        // go trough each parent until we get to a root page (which has it's parent ID set to the rootPageId)

                        var parent = LoadedPages.FirstOrDefault(x => x.Page.PrimaryKey == page.Page.ParentId);
                        while (parent != null && parent.Page.PrimaryKey != RootParentId) {
                            // add the parent to the list of pages to be pushed
                            pagesToPush.Add(parent);
                            // get the next parent
                            parent = LoadedPages.FirstOrDefault(x => x.Page.PrimaryKey == parent.Page.ParentId);
                        }

                        // go to the list in reverse order since the deepest element is at i = 0 (which is the page we want to show)
                        for (var i = pagesToPush.Count - 1; i >= 0; i--) {
                            OnPageTapped(pagesToPush[i]);
                        }

                    }
                }


                IsBusy = false;
            }
        }

        /// <summary>
        /// Sets the root pages.
        /// </summary>
        private void SetRootPages() {
            //var id = SelectedPage?.Page?.PrimaryKey ?? "0";
            var key = RootParentId;
            RootPages = new ObservableCollection<PageViewModel>(LoadedPages.Where(x => x.Page.ParentId == key).OrderBy(x => x.Page.Order));
        }


        /// <summary>
        /// Sets the children properties for each given page.
        /// </summary>
        /// <param name="onPages">The target pages.</param>
        private void SetChildrenProperties(IList<PageViewModel> onPages) {
            // go through each page and set the children list
            foreach (var pageViewModel in onPages) {
                pageViewModel.Children = onPages.Where(x => x.Page.ParentId == pageViewModel.Page.PrimaryKey).ToList();
            }
        }

        public void OnPagePopped(object sender, NavigationEventArgs e) {
            if (_shownPages != null && _shownPages.Count > 0)
                _shownPages.Pop();
        }
    }
}
