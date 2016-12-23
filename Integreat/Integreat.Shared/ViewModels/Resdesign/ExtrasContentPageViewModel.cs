using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class ExtrasContentPageViewModel : BaseViewModel {
        private INavigator _navigator;

        public ExtrasContentPageViewModel(IAnalyticsService analytics, INavigator navigator)
        : base(analytics) {
            Title = "Extras";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

    }
}
