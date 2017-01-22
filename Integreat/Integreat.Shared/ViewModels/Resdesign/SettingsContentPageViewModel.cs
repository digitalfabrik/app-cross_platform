using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class SettingsContentPageViewModel : BaseViewModel {
        private INavigator _navigator;

        public SettingsContentPageViewModel(IAnalyticsService analytics, INavigator navigator)
        : base(analytics) {
            Title = "Settings";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

    }
}
