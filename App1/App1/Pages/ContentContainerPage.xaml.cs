using System;
using System.Security;
using App1.Utilities;
using App1.ViewModels;
using Xamarin.Forms;

namespace App1.Pages
{
    /// <summary>
    /// This is the base container for the different Pages (Events, Categories and Extras)
    /// </summary>
    [SecurityCritical]
    public partial class ContentContainerPage
    {
        private ContentContainerViewModel _vm;

        [SecurityCritical]
        public ContentContainerPage()
        {
            InitializeComponent();
            BindingContextChanged += OnBindingContextChanged;
            Appearing += OnAppearing;

        }
        [SecurityCritical]
        private void OnAppearing(object sender, EventArgs eventArgs)
        {
            if (_vm == null) return;

#if __ANDROID__
            Appearing -= OnAppearing;
#endif

            var locationId = Preferences.Location();
            if (locationId < 0 || Preferences.Language(locationId).IsNullOrEmpty())
            {
                // not language / location selected
                _vm.OpenLocationSelection();

                //abort so that the user can select a location
                return;
            }

            _vm.CreateMainView(Children);

            CurrentPage = Children[1];

#if __ANDROID__
            CurrentPageChanged += (o, args) => UpdateToolbarItems();

            ((NavigationPage)Application.Current.MainPage).Pushed += (o, args) => UpdateToolbarItems();
            ((NavigationPage)Application.Current.MainPage).Popped += (o, args) => UpdateToolbarItems();

            UpdateToolbarItems();
#endif
        }

        /// <summary>
        /// Updates the toolbar items.
        /// </summary>
        private void UpdateToolbarItems()
        {
            var activeChild = CurrentPage;
            if (!(activeChild.BindingContext is BaseContentViewModel)) return;

            try
            {
                var toolbarItems = ((BaseContentViewModel)activeChild.BindingContext).ToolbarItems;
                var navigationPage = (NavigationPage)Application.Current.MainPage;

                //current shown page
                var crntPage = navigationPage.CurrentPage;

                //clear the current items
                navigationPage.ToolbarItems.Clear();
                navigationPage.ToolbarItems.AddRange(toolbarItems);
            }
            catch(Exception)
            {
                //ignored
            }
        }


        [SecurityCritical]
        private void OnBindingContextChanged(object sender, EventArgs eventArgs)
        {
            if (!(BindingContext is ContentContainerViewModel vm)) return;
            _vm = vm;
            BindingContextChanged -= OnBindingContextChanged;
        }
    }
}
