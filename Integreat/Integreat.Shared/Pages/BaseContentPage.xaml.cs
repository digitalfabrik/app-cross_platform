using System;
using System.Security;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    /// <summary>
    /// Base class for all content pages
    /// </summary>
    [SecurityCritical]
    public partial class BaseContentPage : ContentPage
    {
        [SecurityCritical]
        public BaseContentPage()
        {
            BackgroundColor = Color.White;
            BindingContextChanged += OnBindingContextChanged;
        }
        [SecurityCritical]
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is BaseViewModel viewModel && viewModel.OnAppearingCommand.CanExecute(null))
            {
                viewModel.OnAppearingCommand.Execute(null);
            }
        }

        /// <summary>
        /// Calls the RefreshCommand on this Page' ViewModel. Implying that the ViewModel is a BaseViewModel
        /// </summary>
        /// <param name="metaDataChanged">Whether the meta data (that is language and/or location) has changed.</param>
        public void Refresh(bool metaDataChanged = false)
        {
            var viewModel = BindingContext as BaseViewModel;

            if (viewModel?.RefreshCommand.CanExecute(null) != true)
            {
                return;
            }

            viewModel.RefreshCommand.Execute(false);
        }

        /// <summary>
        /// Called when [binding context changed]. Used to pass the Navigation to the ViewModels which for this Page.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBindingContextChanged(object sender, EventArgs eventArgs)
        {
            if (!(BindingContext is BaseViewModel vm)) return;
            vm.Navigation = Navigation;
        }
    }
}
