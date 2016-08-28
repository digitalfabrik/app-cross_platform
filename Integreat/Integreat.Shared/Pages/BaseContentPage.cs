using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public class BaseContentPage : ContentPage
    {
		public BaseContentPage() {
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as BaseViewModel;
            if (viewModel != null && viewModel.OnAppearingCommand.CanExecute(null))
            {
                viewModel.OnAppearingCommand.Execute(null);
            }
        }
    }
}
