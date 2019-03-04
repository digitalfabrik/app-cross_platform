using System;
using Integreat.Shared.Services;

namespace Integreat.Shared.ViewModels.Events
{
    /// <summary>
    /// Class EventsSingleItemDetailViewModel contains information and functionality for EventSinglePages
    /// </summary>
    public class EventsSingleItemDetailViewModel : BaseWebViewViewModel
    {
        private EventPageViewModel _pageToShow;

        public EventsSingleItemDetailViewModel(INavigator navigator,
            Func<string, ImagePageViewModel> imagePageFactory,
            Func<string, PdfWebViewPageViewModel> pdfWebViewFactory,
            EventPageViewModel pageToShow, MainContentPageViewModel mainContentPageViewModel)
            : base(navigator, imagePageFactory, pdfWebViewFactory, mainContentPageViewModel)
        {
            PageToShow = pageToShow;
            Title = pageToShow.EventTitle;
        }

        /// <summary> Gets or sets the event page to show. </summary>
        /// <value> The page to show. </value>
        public EventPageViewModel PageToShow
        {
            get => _pageToShow;
            set => SetProperty(ref _pageToShow, value);
        }

        /// <summary> Gets the source. </summary>
        /// <value> The source. </value>
        public string Source => _pageToShow.EventContent;
    }
}
