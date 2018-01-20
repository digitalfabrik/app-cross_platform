using System;
using System.Security;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
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
            // we don't want this to build twice, so we remove the event listener
            Appearing -= OnAppearing;

            var locationId = Preferences.Location();
            if (locationId < 0 || Preferences.Language(locationId).IsNullOrEmpty())
            {
                // not language / location selected
                _vm.OpenLocationSelection();

                _vm.LanguageSelected -= VmOnLanguageSelected; // ensure not to subscribe twice
                _vm.LanguageSelected += VmOnLanguageSelected;
                return;
            }

            _vm.CreateMainView(Children, (NavigationPage)Application.Current.MainPage);

            CurrentPage = Children[1];

            // when the current child (Extras, Main, Events) is changed,
            CurrentPageChanged += (o, args) => UpdateToolbarItems();
            // or a page has been pushed or popped off the main navigation, update the toolbar
            ((NavigationPage)Application.Current.MainPage).Pushed += (o, args) => UpdateToolbarItems();
            ((NavigationPage)Application.Current.MainPage).Popped += (o, args) => UpdateToolbarItems();

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
                var childItems = ((BaseContentViewModel)activeChild.BindingContext).ToolbarItems;
                var defaultItems = ((ContentContainerViewModel)BindingContext).DefaultToolbarItems;
                var navigationPage = (NavigationPage)Application.Current.MainPage;

                // current shown page
                var crntPage = navigationPage.CurrentPage;

                // clear the current items
                navigationPage.ToolbarItems.Clear();
#if __IOS__
                ToolbarItems.Clear();
#endif
                // add the child items only if the current shown page is the contentContainer
                if (childItems != null && crntPage == this)
                    navigationPage.ToolbarItems.AddRange(childItems);

                // add the default items
                if (defaultItems != null)
                {
#if __IOS__
                    ToolbarItems.AddRange(defaultItems);
#else
                    navigationPage.ToolbarItems.AddRange(defaultItems);
#endif
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [SecurityCritical]
        private void VmOnLanguageSelected(object sender, EventArgs eventArgs)
        {
            if (_vm != null)
                _vm.LanguageSelected -= VmOnLanguageSelected;
            OnAppearing(sender, eventArgs);
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
