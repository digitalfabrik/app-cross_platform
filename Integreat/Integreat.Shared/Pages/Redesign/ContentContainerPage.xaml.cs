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

            _vm.CreateMainView(Children, (NavigationPage) Application.Current.MainPage);

            CurrentPage = Children[1];

            // when the current child (Extras, Main, Events) is changed,
            CurrentPageChanged += (o, args) => UpdateToolbarItems();
            // or a page has been pushed or popped off the main navigation, update the toolbar
            ((NavigationPage)Application.Current.MainPage).Pushed += (o, args) => UpdateToolbarItems();
            ((NavigationPage) Application.Current.MainPage).Popped += (o, args) => UpdateToolbarItems();

            // initial toolbar initialization after creating the main view
            UpdateToolbarItems();
        }


        /// <summary>
        /// Updates the toolbar items.
        /// </summary>
        private void UpdateToolbarItems()
        {
            var activeChild = CurrentPage;
            if (!(activeChild.BindingContext is BaseContentViewModel)) return;

            // get the toolbar items of the current child and the default items, merge them and set them to be shown
            try
            {
                // the following three statements are all MVVM breaking, structure assuming calls, so we try/catch it just in case
                var childItems = ((BaseContentViewModel) activeChild.BindingContext).ToolbarItems;
                var defaultItems = ((ContentContainerViewModel) BindingContext).DefaultToolbarItems;
                var navigationPage = (NavigationPage)Application.Current.MainPage;

                // current shown page
                var crntPage = navigationPage.CurrentPage;

                // clear the current items
                navigationPage.ToolbarItems.Clear();

                // add the child items only if the current shown page is the contentContainer
                if(childItems != null && crntPage == this)
                    navigationPage.ToolbarItems.AddRange(childItems);

                // add the default items
                if(defaultItems != null) 
                    navigationPage.ToolbarItems.AddRange(defaultItems);

            }
            catch (Exception)
            {
                // ignored
            }
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
