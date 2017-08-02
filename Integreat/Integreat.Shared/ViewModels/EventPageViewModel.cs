using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels
{
    public class EventPageViewModel : PageViewModel
    {
        private readonly EventPage _eventPage;
        private string _eventContent;

        public EventPageViewModel(IAnalyticsService analytics, INavigator navigator, EventPage page, IDialogProvider dialogProvider) : base(analytics, navigator, page, dialogProvider)
        {
            _eventPage = page;
        }

        public string EventThumbnail => _eventPage.EventThumbnail;
        public string EventLocation => _eventPage.Location.Address + ", " + _eventPage.Location.Town;
        public string EventDate => _eventPage.EventDate;
        public string EventTitle => _eventPage.Title;
        public string EventDescriptionShort
        {
            get
            {
                const int shortStringLength = 120;
                var strLength = _eventPage.Description.Length;
                if(strLength > shortStringLength) {return _eventPage.Description.Substring(0, shortStringLength) + "...";}
                return _eventPage.Description;
            }
        }
        public string EventRowTwo => EventDate + ", " + EventLocation;
        /// <summary>
        /// this content is used to add addtional information like date, location, etc. for an event
        /// </summary>
        public string EventContent
        {
            get => _eventContent;
            set => SetProperty(ref _eventContent, value);
        }

    }
}
