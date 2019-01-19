﻿using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Integreat.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// Class MainContentPageViewModel holds all information related to the main categorie view
    /// </summary>
    public class MainContentPageViewModel : BaseContentViewModel
    {
        #region Fields

        private readonly INavigator _navigator;

        private readonly Func<Page, PageViewModel> _pageViewModelFactory; // creates PageViewModel's out of Pages
        private IList<PageViewModel> _loadedPages = new List<PageViewModel>();

        private readonly Func<PageViewModel, IList<PageViewModel>, MainTwoLevelViewModel> _twoLevelViewModelFactory
            ; // factory which creates ViewModels for the two level view


        private readonly Func<IEnumerable<PageViewModel>, SearchViewModel> _pageSearchViewModelFactory;
        private ObservableCollection<PageViewModel> _rootPages = new ObservableCollection<PageViewModel>();
        private ICommand _itemTappedCommand;
        private ICommand _changeLanguageCommand;
        private ICommand _openSearchCommand;
        private ICommand _onOpenContactsCommand;
        private readonly IDialogProvider _dialogProvider;
        private ContentContainerViewModel _contentContainer;
        private readonly Stack<PageViewModel> _shownPages;
        private int _pageIdToShowAfterLoading;
        private readonly DataLoaderProvider _dataLoaderProvider;
        private readonly IViewFactory _viewFactory;
        private readonly Func<string, GeneralWebViewPageViewModel> _generalWebViewFactory;

        #endregion

        public MainContentPageViewModel(INavigator navigator,
                                        DataLoaderProvider dataLoaderProvider,
                                        Func<Page, PageViewModel> pageViewModelFactory,
                                        IDialogProvider dialogProvider,
                                        Func<PageViewModel, IList<PageViewModel>, MainTwoLevelViewModel> twoLevelViewModelFactory
                                        , Func<IEnumerable<PageViewModel>, SearchViewModel> pageSearchViewModelFactory
                                        , IViewFactory viewFactory, Func<string, GeneralWebViewPageViewModel> generalWebViewFactory)
                                        : base(dataLoaderProvider)
        {
            Title = AppResources.Categories;
            Icon = Device.RuntimePlatform == Device.Android ? null : "home150";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _dataLoaderProvider = dataLoaderProvider;
            _pageViewModelFactory = pageViewModelFactory;
            _twoLevelViewModelFactory = twoLevelViewModelFactory;
            _dialogProvider = dialogProvider;
            _pageSearchViewModelFactory = pageSearchViewModelFactory;
            _viewFactory = viewFactory;
            _generalWebViewFactory = generalWebViewFactory;

            _shownPages = new Stack<PageViewModel>();

            ItemTappedCommand = new Command(OnPageTapped);
            OpenSearchCommand = new Command(OnOpenSearch);
            ChangeLanguageCommand = new Command(OnChangeLanguage);
            OpenContactsCommand = new Command(OnOpenContacts);

            ShowHeadline = Device.RuntimePlatform != Device.Android;

            // add toolbar items
            ToolbarItems = GetPrimaryToolbarItemsComplete(OpenSearchCommand, ChangeLanguageCommand);

            RootPages = new ObservableCollection<PageViewModel>();
        }


        #region Properties

        /// <summary> Gets or sets the loaded pages. (I.e. all pages for the selected region/language) </summary>
        /// <value> The loaded pages. </value>
        public IList<PageViewModel> LoadedPages
        {
            get => _loadedPages;
            set => SetProperty(ref _loadedPages, value);
        }

        /// <summary> Gets or sets the root pages. That are all pages without parents. </summary>
        /// <value> The root pages. </value>
        public ObservableCollection<PageViewModel> RootPages
        {
            get => _rootPages;
            set => SetProperty(ref _rootPages, value);
        }

        /// <summary> Gets or sets the item tapped command. </summary>
        /// <value> The item tapped command. </value>
        public ICommand ItemTappedCommand
        {
            get => _itemTappedCommand;
            set => SetProperty(ref _itemTappedCommand, value);
        }

        /// <summary> Gets or sets the open search command. </summary>
        /// <value> The open search command. </value>
        public ICommand OpenSearchCommand
        {
            get => _openSearchCommand;
            set => SetProperty(ref _openSearchCommand, value);
        }


        /// <summary> Gets or sets the open contacts command. </summary>
        /// <value> The open contacts command. </value>
        public ICommand OpenContactsCommand
        {
            get => _onOpenContactsCommand;
            set => SetProperty(ref _onOpenContactsCommand, value);
        }

        /// <summary> Gets or sets the change language command. </summary>
        /// <value> The change language command. </value>
        public ICommand ChangeLanguageCommand
        {
            get => _changeLanguageCommand;
            set => SetProperty(ref _changeLanguageCommand, value);
        }

        /// <summary> Gets or sets the content container. </summary>
        /// <value> The content container. </value>
        public ContentContainerViewModel ContentContainer
        {
            get => _contentContainer;
            set => SetProperty(ref _contentContainer, value);
        }

        #endregion
        private async void OnOpenContacts(object obj)
        {
            if (IsBusy) return;

            string content;
            try
            {
                IsBusy = true;

                var pages = await _dataLoaderProvider.DisclaimerDataLoader.Load(true, LastLoadedLanguage,
                    LastLoadedLocation);
                content = string.Join("<br><br>", pages.Select(x => x.Content));
            }
            finally
            {
                IsBusy = false;
            }

            var viewModel = _generalWebViewFactory(content);
            //trigger load content
            viewModel?.RefreshCommand.Execute(false);
            await _navigator.PushAsync(viewModel, Navigation);
        }

        private async void OnChangeLanguage(object obj)
        {
            if (IsBusy) return;

            // if there are no pages in the stack, it means we're in root. Show the normal language selection
            if (_shownPages.IsNullOrEmpty())
            {
                ContentContainer.OpenLanguageSelection(false);
                return;
            }

            // get the current shown page
            var pageModel = _shownPages.Peek().Page;
            if (pageModel.AvailableLanguages.IsNullOrEmpty())
            {
                return; // abort if there are no other languages available
            }

            // get the languages the page is available in. These only contain short names and ids (not keys), therefore we need to parse them a bit
            var languageShortNames = pageModel.AvailableLanguages.Select(x => x.Id).ToList();

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
            if (selectedLanguage != null)
            {
                // load and show page. Get the page Id and generate the page key
                var parentPage = pageModel.AvailableLanguages.FirstOrDefault(l => l.Id.Contains(selectedLanguage.ShortName))?.ParentPage;

                if (parentPage != null)
                {
                    var otherPageId = parentPage.Id;
                    _pageIdToShowAfterLoading = otherPageId;
                }

                await Navigation.PopToRootAsync();
                _shownPages.Clear();

                // set new language
                Preferences.SetLanguage(Preferences.Location(), selectedLanguage);
                ContentContainer.RefreshAll(true);
            }
            else
            {
                Debug.Write("No language selected");
            }
        }

        private async void OnOpenSearch(object obj)
        {
            if (IsBusy) return;

            await _navigator.PushAsync(_pageSearchViewModelFactory(LoadedPages), Navigation);
        }

        private async Task<IEnumerable<Language>> LoadLanguages()
        {
            return await _dataLoaderProvider.LanguagesDataLoader.Load(false, LastLoadedLocation ?? (LastLoadedLocation =
                                                                                 (await _dataLoaderProvider
                                                                                     .LocationsDataLoader.Load(false))
                                                                                 .FirstOrDefault(x =>
                                                                                     x.Id == Preferences.Location())));
        }

        /// <summary> Called when the user [tap]'s on a item. </summary>
        /// <param name="pageViewModel">The view model of the clicked page item.</param>
        public async void OnPageTapped(object pageViewModel)
        {
            if (!(pageViewModel is PageViewModel pageVm)) return;

            //check if meta-tag already exists
            if (pageVm.HasContent && !pageVm.Content.StartsWith(HtmlTags.Doctype.GetStringValue()
                                            + Constants.MetaTagBuilderTag, StringComparison.Ordinal))
            {
                var mb = new MetaTagBuilder(pageVm.Content);
                mb.MetaTags.ToList().AddRange(
                    new List<string>
                    {
                        "<meta name='viewport' content='width=device-width'>",
                        "<meta name='format-detection' content='telephone=no'>"
                    });
                pageVm.Page.Content = mb.Build();
            }
            _shownPages.Push(pageVm);

            //if it is page without children and content, than display html page
            if (!pageVm.Children.Any() && pageVm.HasContent)
            {
                await NavigateToContentPage(pageVm);
                return;
            }
            //if it is root page, display menu as next page
            if (RootPages.Contains(pageVm))
            {
                // target page has children, display another two level view
                await NavigateToTwoLevelMenuPage(pageVm);
                return;
            }
            //if it is not root page and has no content but has childs display menu as next page
            if (!RootPages.Contains(pageVm) && !pageVm.HasContent && pageVm.Children.Count > 0)
            {
                // target page has children, display another two level view
                await NavigateToTwoLevelMenuPage(pageVm);
                return;
            }
            //if it is not root and has content display html page
            if (RootPages.Contains(pageVm) || !pageVm.HasContent) return;
            // target page has no children, display only content
            await NavigateToContentPage(pageVm);
        }

        private async Task NavigateToTwoLevelMenuPage(PageViewModel pageVm)
        {
            await _navigator.PushAsync(_twoLevelViewModelFactory(pageVm, LoadedPages), Navigation);
        }

        private async Task NavigateToContentPage(PageViewModel pageVm)
        {
            var vm = _generalWebViewFactory(pageVm.Content);
            var view = _viewFactory.Resolve(vm);
            view.Title = pageVm.Title;
            await Navigation.PushAsync(view);
            vm.NavigatedTo();
        }

        /// <inheritdoc />
        /// <summary> Loads all pages for the given language and location from the persistenceService. </summary>
        protected override async void LoadContent(bool forced = false, Language forLanguage = null,
            Location forLocation = null)
        {
            SetLanguageAndLocationIfNull(ref forLanguage, ref forLocation);

            if (IsBusy || forLocation == null || forLanguage == null)
            {
                AbortIfPreconditionsFail();
                return;
            }

            try
            {
                IsBusy = true;
                LoadedPages?.Clear();
                RootPages?.Clear();
                var pages = await _dataLoaderProvider.PagesDataLoader.Load(forced, forLanguage, forLocation,
                    err => ErrorMessage = err);

                LoadPagesAndRegisterCommands(pages);

                // set children
                SetChildrenProperties(LoadedPages);

                SetRootPages();
            }
            finally
            {
                if (_pageIdToShowAfterLoading != 0 && LoadedPages != null)
                {
                    // find page id
                    var page = LoadedPages.FirstOrDefault(x => x.Page.Id == _pageIdToShowAfterLoading);
                    _pageIdToShowAfterLoading = 0;

                    if (page != null)
                    {
                        var pagesToPush = new List<PageViewModel> { page };
                        // go trough each parent until we get to a root page (which has it's parent ID set to the rootPageId)

                        var parent = LoadedPages.FirstOrDefault(x => x.Page.Id == page.Page.ParentPage.Id);
                        while (parent != null && parent.Page.Id != 0)
                        {
                            // add the parent to the list of pages to be pushed
                            pagesToPush.Add(parent);
                            // get the next parent
                            parent = LoadedPages.FirstOrDefault(x => x.Page.Id == parent.Page.ParentPage.Id);
                        }

                        // go to the list in reverse order since the deepest element is at i = 0 (which is the page we want to show)
                        for (var i = pagesToPush.Count - 1; i >= 0; i--)
                        {
                            OnPageTapped(pagesToPush[i]);
                        }
                    }
                }
                IsBusy = false;
            }
        }

        private void LoadPagesAndRegisterCommands(IEnumerable<Page> pages)
        {
            LoadedPages = pages.Select(page => _pageViewModelFactory(page)).ToList();

            // register commands
            foreach (var pageViewModel in LoadedPages)
            {
                pageViewModel.OnTapCommand = new Command(OnPageTapped);
            }
        }

        private void AbortIfPreconditionsFail()
        {
            Debug.WriteLine("LoadPages could not be executed");
            if (IsBusy) Debug.WriteLine("The app is busy");
            if (null == LastLoadedLanguage) Debug.WriteLine("Language is null");
            if (null == LastLoadedLocation) Debug.WriteLine("Location is null");
        }

        private void SetLanguageAndLocationIfNull(ref Language forLanguage, ref Location forLocation)
        {
            if (forLocation == null) forLocation = LastLoadedLocation;
            if (forLanguage == null) forLanguage = LastLoadedLanguage;
        }

        /// <summary> Sets the root pages. </summary>
        private void SetRootPages()
        {
            RootPages = new ObservableCollection<PageViewModel>(LoadedPages.Where(x => x.Page.ParentPage.Id == 0)
                .OrderBy(x => x.Page.Order));
        }

        /// <summary> Sets the children properties for each given page. </summary>
        /// <param name="onPages">The target pages.</param>
        private void SetChildrenProperties(IList<PageViewModel> onPages)
        {
            // go through each page and set the children list
            foreach (var pageViewModel in onPages)
            {
                pageViewModel.Children = onPages.Where(x => x.Page.ParentPage.Id == pageViewModel.Page.Id).ToList();
            }
        }

        /// <summary> Called when [page popped]. </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        public void OnPagePopped(object sender, NavigationEventArgs e)
        {
            if (_shownPages != null && _shownPages.Any())
                _shownPages.Pop();
        }
    }
}