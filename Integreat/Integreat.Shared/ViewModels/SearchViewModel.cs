using System;
using System.Linq;
using System.Collections.Generic;
using Integreat.Shared.Services.Tracking;
using localization;

namespace Integreat.Shared.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IEnumerable<PageViewModel> _pages;

        public IList<PageViewModel> FoundPages {
            get { return _foundPages; }
            set { SetProperty(ref _foundPages, value); }
        }

        public SearchViewModel(IAnalyticsService analytics, IEnumerable<PageViewModel> pages)
            : base(analytics)
        {
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

        #region View Data

        private string _searchText = string.Empty;
        private IList<PageViewModel> _foundPages;

        public string SearchText {
            get { return _searchText; }
            set {
                if (SetProperty(ref _searchText, value))
                {
                    Search();
                }
            }
        }
        #endregion

        #region Commands

        public void Search()
        {
            IsBusy = true;
            var found = _pages.Where(x => x.Page.Find(SearchText)).ToList();
            found.Sort(Comparison);
            FoundPages = found;
            IsBusy = false;
        }

        /// <summary>
        /// Comparisons function for two pages.
        /// </summary>
        /// <param name="pageA">The first page a.</param>
        /// <param name="pageB">The second page b.</param>
        /// <returns>An integer that indicates the lexical relationship between the two comparands.</returns>
        private int Comparison(PageViewModel pageA, PageViewModel pageB) => string.CompareOrdinal(pageA.Title, pageB.Title);

        #endregion
    }
}
