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

            // when the current child (Extras, Main, Events) is changed,
            CurrentPageChanged += (o, args) => UpdateToolbarItems();

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
                var navigationPage = (MainNavigationPage)Application.Current.MainPage;

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
                    foreach(ToolbarItem item in defaultItems)
                    {
                        if(item.Order == ToolbarItemOrder.Primary){
                            ToolbarItems.Add(item);
                        }
                    }
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
        private void OnBindingContextChanged(object sender, EventArgs eventArgs)
        {
            if (!(BindingContext is ContentContainerViewModel vm)) return;
            _vm = vm;
            BindingContextChanged -= OnBindingContextChanged;
        }
    }
}
