using System.Security;

namespace Integreat.Shared.Pages
{
    /// <summary>
    /// The event content page is the entry page for the events there each event is listed
    /// </summary>
    public partial class EventsContentPage
	{
	    [SecurityCritical]
        public EventsContentPage ()
		{
			InitializeComponent ();
		}
	}
}
