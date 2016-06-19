using System.Collections.Generic;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels
{
    public class DetailedPagesViewModel : BaseViewModel
    {
        private IEnumerable<PageViewModel> _pages;
        public IEnumerable<PageViewModel> Pages
        {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }
        
        public string Content { get; set; }

        public DetailedPagesViewModel(IAnalyticsService analytics, PageViewModel parentPage, IEnumerable<PageViewModel> pages)
            : base(analytics)
        {
            Title = parentPage.Title;
            _pages = pages;
            Content = parentPage.Page.Content;
        }
    }
}
