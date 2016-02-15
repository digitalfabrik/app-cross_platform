using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels
{
    public class EventPageViewModel : PageViewModel
    {
        public EventPageViewModel(IAnalyticsService analytics, INavigator navigator, EventPage page, IDialogProvider dialogProvider) : base(analytics, navigator, page, dialogProvider)
        {
        }
    }
}
