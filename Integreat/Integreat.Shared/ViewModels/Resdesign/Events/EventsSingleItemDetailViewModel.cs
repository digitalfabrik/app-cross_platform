using System;
using System.Collections.Generic;
using System.Text;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.ViewModels.Resdesign.Events
{
    public class EventsSingleItemDetailViewModel : BaseViewModel
    {
        private EventPageViewModel _pageToShow;

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
