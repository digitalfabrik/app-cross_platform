using System.Collections.Generic;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.ViewModels.Resdesign;

namespace Integreat.Shared.ViewModels {
    public class MainTwoLevelViewModel : BaseViewModel {
        #region Fields

        private IList<PageViewModel> _pages;
        private MainContentPageViewModel _mainContentPageViewModel; // the view model for the mainContentPage used to open new pages

        #endregion

        #region Properties

        public IList<PageViewModel> Pages {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }
        

        #endregion

        public MainTwoLevelViewModel(IAnalyticsService analytics, PageViewModel parentPage, IList<PageViewModel> pages, MainContentPageViewModel mainContentPageViewModel)
            : base(analytics) {
            Title = parentPage.Title;
            _pages = pages;
            _mainContentPageViewModel = mainContentPageViewModel;
        }
    }
}
