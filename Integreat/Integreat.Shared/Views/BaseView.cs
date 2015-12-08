using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    public class BaseView : ContentPage
    {
        public BaseView()
        {
            SetBinding(TitleProperty, new Binding(BaseViewModel.TitlePropertyName));
            SetBinding(IconProperty, new Binding(BaseViewModel.IconPropertyName));
        }
    }
}
