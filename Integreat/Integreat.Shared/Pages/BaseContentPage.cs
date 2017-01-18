using System;
using Integreat.Shared.ViewModels;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;

namespace Integreat.Shared.Pages {
    public class BaseContentPage : ContentPage {
        public BaseContentPage() {
            //Margin on top for the ios navigation bar
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            BackgroundColor = Color.White;
            BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            var viewModel = BindingContext as BaseViewModel;
            if (viewModel != null && viewModel.OnAppearingCommand.CanExecute(null)) {
                viewModel.OnAppearingCommand.Execute(null);
            }
        }

        /// <summary>
        /// Calls the RefreshCommand on this Page' ViewModel. Implying that the ViewModel is a BaseViewModel
        /// </summary>
        public void Refresh() {
            var viewModel = BindingContext as BaseViewModel;
            if (viewModel?.RefreshCommand.CanExecute(null) != true) return;
            viewModel.RefreshCommand.Execute(null);
        }


        /// <summary>
        /// Called when [binding context changed]. Used to pass the Navigation to the ViewModels which for this Page.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnBindingContextChanged(object sender, EventArgs eventArgs) {
            var vm = BindingContext as BaseViewModel;
            if (vm == null) return;
            vm.Navigation = Navigation;
        }

    }
}
