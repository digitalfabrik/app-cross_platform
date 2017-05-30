using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Redesign {
    [SecurityCritical]
    public partial class ContentContainerPage
    {
        private ContentContainerViewModel _vm;

        [SecurityCritical]
        public ContentContainerPage() {
            InitializeComponent();
            BindingContextChanged += OnBindingContextChanged;
            //  CurrentPageChanged += OnCurrentPageChanged;
            Appearing += OnAppearing;
            
        }
        [SecurityCritical]
        private void OnAppearing(object sender, EventArgs eventArgs) {
            if (_vm == null) return;
            // we don't want this to build twice, so we remove the event listener
            Appearing -= OnAppearing;

            var locationId = Preferences.Location();
            if (locationId < 0 || Preferences.Language(locationId).IsNullOrEmpty()) {
                // not language / location selected
                _vm.OpenLocationSelection();

                _vm.LanguageSelected -= VmOnLanguageSelected; // ensure not to subscribe twice
                _vm.LanguageSelected += VmOnLanguageSelected;
                return;
            }

            _vm.CreateMainView(Children, Application.Current.MainPage as NavigationPage);
            CurrentPage = Children[1];
        }
        [SecurityCritical]
        private void VmOnLanguageSelected(object sender, EventArgs eventArgs) {
            if (_vm != null)
                _vm.LanguageSelected -= VmOnLanguageSelected;
            OnAppearing(sender, eventArgs);
        }

        /*   private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
           {
               var asPage = sender as ContentContainerPage;
               var contentAsNavigationPage = asPage?.CurrentPage as NavigationPage;
               if (contentAsNavigationPage == null) return;
               ForceLayout();
               NavigationPage.SetHasBackButton(Application.Current.MainPage, true);
           }*/
        [SecurityCritical]
        private void OnBindingContextChanged(object sender, EventArgs eventArgs) {
            var vm = BindingContext as ContentContainerViewModel;
            if (vm == null) return;
            _vm = vm;
            BindingContextChanged -= OnBindingContextChanged;
        }
    }
}
