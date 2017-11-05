using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.ViewModels.Resdesign.Events
{
    public class EventsSingleItemDetailViewModel : BaseWebViewViewModel
    {
        private EventPageViewModel _pageToShow;

        /// <summary> Gets or sets the event page to show. </summary>
        /// <value> The page to show. </value>
        public EventPageViewModel PageToShow
        {
            get => _pageToShow;
            set => SetProperty(ref _pageToShow, value);
        }

        public bool IsHtmlRawView => Preferences.GetHtmlRawViewSetting();

        public string Content => _pageToShow.EventContent;

        public EventsSingleItemDetailViewModel(IAnalyticsService analyticsService, EventPageViewModel pageToShow) : base(analyticsService)
        {
            PageToShow = pageToShow;
            Title = pageToShow.EventTitle;
        }
    }
}
