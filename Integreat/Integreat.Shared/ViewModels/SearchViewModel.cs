using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Integreat.Shared.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly IEnumerable<PageViewModel> _pages;

        public SearchViewModel(IEnumerable<PageViewModel> pages)
        {
            if (pages == null)
            {
                throw new ArgumentNullException(nameof(pages));
            }
            Title = "Search";
            _pages = pages;
            FoundPages = new ObservableCollection<PageViewModel>();
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

        public ObservableCollection<PageViewModel> FoundPages { get; set; }

        #endregion

        #region Commands

        public void Search()
        {
            FoundPages.Clear();
            FoundPages.AddRange(_pages.Where(x => x.Page.find(SearchText)));
        }

        #endregion
    }
}
