using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign
{
	public partial class EventsContentPage : BaseContentPage
	{
	    [SecurityCritical]
        public EventsContentPage ()
		{
			InitializeComponent ();
		}
	}
}
