using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Tracking;
using System;

namespace Integreat.Shared.ViewModels
{
    public class EventPageViewModel : PageViewModel
    {
        public EventPageViewModel(IAnalyticsService analytics, INavigator navigator, EventPage page, IDialogProvider dialogProvider, Func<Language, Location, PageLoader> pageLoaderFactory) : base(analytics, navigator, page, dialogProvider, pageLoaderFactory)
        {
        }
    }
}
