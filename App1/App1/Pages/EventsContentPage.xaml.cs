using System.Security;

namespace App1.Pages
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
