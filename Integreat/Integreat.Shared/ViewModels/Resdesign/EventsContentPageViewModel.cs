using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign {
    public class EventsContentPageViewModel : BaseViewModel {
        private INavigator _navigator;

        public EventsContentPageViewModel(IAnalyticsService analytics, INavigator navigator)
        : base(analytics) {
            Title = "Events";
            _navigator = navigator;
            _navigator.HideToolbar(this);
        }

    }
}
