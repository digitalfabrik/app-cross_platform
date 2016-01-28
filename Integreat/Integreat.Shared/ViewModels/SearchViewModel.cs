using System;
using System.Linq;
using System.Collections.ObjectModel;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        PageSearch search;
        PagesViewModel pageViewModel;

        public SearchViewModel(PageSearch search, PagesViewModel pageViewModel)
        {
            if (search == null) { 
                throw new ArgumentNullException("search");
            }
            this.search = search;
            this.search.Results = pageViewModel.VisiblePages;

            if (pageViewModel == null)
            {
                throw new ArgumentNullException("pageViewModel");
            }
            this.pageViewModel = pageViewModel;
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

        public Collection<Models.Page> Pages {
            get {
                return search.Results;
            }
        }


        #endregion

        #region Commands

        public void Search()
        {
            search.Results = new Collection<Models.Page>(pageViewModel.LoadedPages.Where(x => x.find(SearchText)).ToList());
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
