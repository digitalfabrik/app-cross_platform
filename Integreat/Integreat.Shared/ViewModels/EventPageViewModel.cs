using Integreat.Shared.Models;
using Integreat.Shared.Services;

namespace Integreat.Shared.ViewModels
{
    public class EventPageViewModel : PageViewModel
    {
        public EventPageViewModel(INavigator navigator, EventPage page, IDialogProvider dialogProvider) : base(navigator, page, dialogProvider)
        {
        }
    }
}
