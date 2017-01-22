using System.Collections.Generic;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.ViewModels.Resdesign;

namespace Integreat.Shared.ViewModels {
    public class MainTwoLevelViewModel : BaseViewModel {
        #region Fields

        private IList<PageViewModel> _pages;
        private PageViewModel _parentPage;
        private MainContentPageViewModel _mainContentPageViewModel;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the pages to be displayed.
        /// </summary>
        public IList<PageViewModel> Pages {
            get { return _pages; }
            private set { SetProperty(ref _pages, value); }
        }

        /// <summary>
        /// Gets or sets the parent page of this TwoLevelView.
        /// </summary>
        public PageViewModel ParentPage
        {
            get { return _parentPage; }
            private set { SetProperty(ref _parentPage, value); }
        }

        /// <summary>
        /// Gets or sets the view model for the mainContentPage used to open new pages
        /// </summary>
        public MainContentPageViewModel MainContentPageViewModel
        {
            get { return _mainContentPageViewModel; }
            private set { SetProperty(ref _mainContentPageViewModel, value); }
        }

        #endregion

        public MainTwoLevelViewModel(IAnalyticsService analytics, PageViewModel parentPage, IList<PageViewModel> pages)
            : base(analytics) {
            Title = parentPage.Title;
            ParentPage = parentPage;
            Pages = pages;
        }
    }
}
