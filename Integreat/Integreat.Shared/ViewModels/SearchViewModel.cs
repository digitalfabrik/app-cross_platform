using System;
using System.Linq;
using System.Collections.Generic;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IEnumerable<PageViewModel> _pages;

        private IEnumerable<PageViewModel> _foundPages;
        public IEnumerable<PageViewModel> FoundPages
        {
            get { return _foundPages; }
            set { SetProperty(ref _foundPages, value); }
        }

        public SearchViewModel(IAnalyticsService analytics, IEnumerable<PageViewModel> pages)
        : base (analytics){
            if (pages == null)
            {
                throw new ArgumentNullException(nameof(pages));
            }
            Title = "Suchen";
            _pages = pages;
            Search();
        }

        #region View Data

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
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
            FoundPages = _pages.Where(x => x.Page.Find(SearchText));
        }

        #endregion
    }
}
