using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign.Main
{
    public class MainSingleItemDetailViewModel : BaseViewModel
    {

        #region Fields

        private PageViewModel _pageToShow;

        #endregion

        #region Properties

        public PageViewModel PageToShow {
            get { return _pageToShow; }
            set { SetProperty(ref _pageToShow, value); }
        }

        #endregion

        public MainSingleItemDetailViewModel(IAnalyticsService analyticsService, PageViewModel pageToShow,  MainContentPageViewModel mainContentPageViewModel) : base(analyticsService)
        {
            PageToShow = pageToShow;
        }

    }
}
