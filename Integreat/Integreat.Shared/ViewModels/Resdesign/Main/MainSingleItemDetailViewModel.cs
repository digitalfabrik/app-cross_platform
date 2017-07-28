using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.ViewModels.Resdesign.Main
{
    public class MainSingleItemDetailViewModel : BaseViewModel
    {

        #region Fields

        private PageViewModel _pageToShow;

        #endregion

        #region Properties

        public PageViewModel PageToShow {
            get => _pageToShow;
            set => SetProperty(ref _pageToShow, value);
        }

        public bool IsHtmlRawView => Preferences.GetHtmlRawViewSetting();
        #endregion

        public MainSingleItemDetailViewModel(IAnalyticsService analyticsService, PageViewModel pageToShow) : base(analyticsService)
        {
            PageToShow = pageToShow;
        }
    }
}
