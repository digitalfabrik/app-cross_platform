using System;
using System.Linq;
using System.Collections.Generic;
using Integreat.Localization;
using Integreat.Shared.ViewFactory;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using System.Windows.Input;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// This ViewModel contains the logic behinde the SearchPage.
    /// </summary>
    public class SearchViewModel : BaseViewModel
    {
        private readonly IEnumerable<PageViewModel> _pages;
        private string _searchText = string.Empty;
        private IList<PageViewModel> _foundPages;
        private readonly IPopupViewFactory _popupViewFactory;
        private readonly Func<string, FeedbackDialogSearchViewModel> _feedbackDialogSearchViewModelFactory;

        private ICommand _openFeedbackCommand;


        public SearchViewModel(IEnumerable<PageViewModel> pages, IPopupViewFactory popupViewFactory, Func<string, FeedbackDialogSearchViewModel> feedbackDialogSearchViewModelFactory)
        {
            _popupViewFactory = popupViewFactory;
            _feedbackDialogSearchViewModelFactory = feedbackDialogSearchViewModelFactory;
            _openFeedbackCommand = new Command(OpenFeedbackDialog);

            if (pages != null)
            {
                Title = AppResources.Search;
                _pages = pages;
                Search();
            }
            else
            {
                throw new ArgumentNullException(nameof(pages));
            }
        }

        /// <summary>
        /// Represents the result of the search.
        /// </summary>
        public IList<PageViewModel> FoundPages
        {
            get => _foundPages;
            set => SetProperty(ref _foundPages, value);
        }

        /// <summary>
        /// The text to filter the pages with.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    Search();
                }
            }
        }

        /// <summary>
        /// Gets or sets the open feedback command.
        /// </summary>
        /// <value>The open feedback command.</value>
        public ICommand OpenFeedbackCommand 
        {
            get => _openFeedbackCommand;
            set => SetProperty(ref _openFeedbackCommand, value);
        }

        /// <summary>
        /// Finds all pages which contain the <c>SearchText</c>.
        /// </summary>
        private void Search()
        {
            IsBusy = true;
            var found = _pages.Where(x => x.Page.Find(SearchText)).ToList();
            found.Sort(Comparison);
            FoundPages = found;
            IsBusy = false;
        }

        /// <summary>
        /// Comparison function for two pages.
        /// </summary>
        /// <param name="pageA">The first page a.</param>
        /// <param name="pageB">The second page b.</param>
        /// <returns>An integer that indicates the lexical relationship between the two comparands.</returns>
        private static int Comparison(PageViewModel pageA, PageViewModel pageB) =>
            string.CompareOrdinal(pageA.Title, pageB.Title);

        private async void OpenFeedbackDialog() {
            var viewModel = _feedbackDialogSearchViewModelFactory(SearchText);
            var view = _popupViewFactory.Resolve(viewModel);
            await PopupNavigation.Instance.PushAsync(view);
        }
    }
}
