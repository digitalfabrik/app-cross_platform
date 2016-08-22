using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public class BaseContentPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 20, 0);
            var viewModel = BindingContext as BaseViewModel;
            if (viewModel != null && viewModel.OnAppearingCommand.CanExecute(null))
            {
                viewModel.OnAppearingCommand.Execute(null);
            }
        }
    }
}
