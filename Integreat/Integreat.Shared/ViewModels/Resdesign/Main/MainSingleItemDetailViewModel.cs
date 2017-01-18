using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign.Main
{
    public class MainSingleItemDetailViewModel : BaseViewModel
    {
        public MainSingleItemDetailViewModel(IAnalyticsService analyticsService, PageViewModel pageToShow,  MainContentPageViewModel mainContentPageViewModel) : base(analyticsService)
        {
        }
    }
}
