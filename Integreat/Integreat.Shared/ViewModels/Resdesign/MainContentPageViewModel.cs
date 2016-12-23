using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class MainContentPageViewModel : BaseViewModel {
        private INavigator _navigator;

        public MainContentPageViewModel(IAnalyticsService analytics, INavigator navigator)
        : base(analytics) {
            Title = "Main content";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

    }
}
