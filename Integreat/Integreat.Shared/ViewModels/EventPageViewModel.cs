using Integreat.Shared.Models;
using Integreat.Shared.Services;
using System.Collections.Generic;

namespace Integreat.Shared.ViewModels
{
    public class EventPageViewModel : PageViewModel
    {
		public EventPageViewModel(INavigator navigator, EventPage page, IDialogProvider dialogProvider, IEnumerable<PageViewModel> pages) 
			: base(navigator, page, dialogProvider, pages)
        {
        }
    }
}
