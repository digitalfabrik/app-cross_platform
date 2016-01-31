using System;
using System.Linq;
using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using System.Collections.Generic;

namespace Integreat.Shared.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        PageSearch search;
        IEnumerable<Page> allPages;

        public SearchViewModel(PageSearch search, IEnumerable<Page> allPages)
        {
            if (search == null) { 
                throw new ArgumentNullException("search");
            }
            this.search = search;

            if (allPages == null)
            {
                throw new ArgumentNullException("allPages");
            }
            this.allPages = allPages;
            this.search.Results = allPages;
        }

        #region View Data

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(search.Name))
                {
                    return "Search";
                }
                else
                {
                    return search.Name;
                }
            }
        }

        public string SearchText
        {
            get { return search.Text; }
            set { search.Text = value ?? ""; }
        }

        public IEnumerable<Page> Pages {
            get {
                return search.Results;
            }
        }


        #endregion

        #region Commands

        public void Search()
        {
            search.Results = new Collection<Page>(allPages.Where(x => x.find(SearchText)).ToList());
            var ev = SearchCompleted;
            if (ev != null)
            {
                ev(this, new SearchCompletedEventArgs
                {
                    SearchText = SearchText,
                });
            }
        }

        #endregion

        #region Events

        public event EventHandler<ErrorEventArgs> Error;

        public event EventHandler<SearchCompletedEventArgs> SearchCompleted;

        #endregion
    }

    public class SearchCompletedEventArgs : EventArgs
    {
        public string SearchText { get; set; }
    }
}
