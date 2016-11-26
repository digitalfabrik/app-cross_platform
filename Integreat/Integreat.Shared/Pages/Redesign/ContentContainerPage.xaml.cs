using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign
{
	public partial class ContentContainerPage : TabbedPage {
		public ContentContainerPage ()
		{
			InitializeComponent ();
            BindingContextChanged += OnBindingContextChanged;
            var dt = BindingContext;
        }

	    private void OnBindingContextChanged(object sender, EventArgs eventArgs) {
            var dt = BindingContext;
        }
	}
}
