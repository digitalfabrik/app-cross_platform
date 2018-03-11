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
