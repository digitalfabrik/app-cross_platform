using System;
using System.Collections.Generic;
using System.Text;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels.Resdesign.Events
{
    public class EventsSingleItemDetailViewModel : BaseViewModel
    {
        private PageViewModel _pageToShow;

        public PageViewModel PageToShow
        {
            get { return _pageToShow; }
            set { SetProperty(ref _pageToShow, value); }
        }

        public EventsSingleItemDetailViewModel(IAnalyticsService analyticsService, PageViewModel pageToShow) : base(analyticsService)
        {
            PageToShow = pageToShow;
        }

        
    }
}
