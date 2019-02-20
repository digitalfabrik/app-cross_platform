using App1.Models.Event;
using App1.Navigator;
using App1.Utilities;

namespace App1.ViewModels
{
    public class EventPageViewModel : PageViewModel
    {
        private readonly EventPage _eventPage;
        private string _eventContent;

        public EventPageViewModel(INavigator navigator, EventPage page)
            : base(navigator, page)
        {
            _eventPage = page;
        }

        public string EventThumbnail => _eventPage.EventThumbnail;
        public string EventLocation => $"{_eventPage.Location.Address}, {_eventPage.Location.Town}";
        public string EventDate => _eventPage.EventDate;
        public string EventTitle => _eventPage.Title;
        public string EventDescriptionShort => _eventPage.Description.StringTruncate(120, "...");

        /// <summary>  Gets the second displayed row on event page. </summary>
        /// <value> The event row two. </value>
        public string EventRowTwo => $"{EventDate}, {EventLocation}";
        /// <summary>This content is used to add additional information like date, location, etc. for an event </summary>
        public string EventContent
        {
            get => _eventContent;
            set => SetProperty(ref _eventContent, value);
        }
    }
}
