using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign
{
	public partial class ContentContainerPage : TabbedPage {
		public ContentContainerPage ()
		{
			InitializeComponent ();
            BindingContextChanged += OnBindingContextChanged;
        }

	    private void OnBindingContextChanged(object sender, EventArgs eventArgs) {
            var vm = BindingContext as ContentContainerViewModel;
	        if (vm == null) return;
            // todo
	    }
	}
}
