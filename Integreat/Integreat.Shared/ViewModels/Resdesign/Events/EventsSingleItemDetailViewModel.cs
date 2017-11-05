using System;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign.General;

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

        public string Source => _pageToShow.EventContent;

        public EventsSingleItemDetailViewModel(IAnalyticsService analyticsService, INavigator navigator, 
            Func<string, ImagePageViewModel> imagePageFactory, 
            Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, 
            EventPageViewModel pageToShow) 
            : base(analyticsService, navigator, imagePageFactory, pdfWebViewFactory)
        {
            PageToShow = pageToShow;
            Title = pageToShow.EventTitle;
        }
    }
}
