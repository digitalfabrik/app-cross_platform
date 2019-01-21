using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Integreat.Shared.ViewFactory;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FeedbackViewModel
    {

        private readonly IPopupViewFactory _viewFactory;
        public ICommand OnFeedbackClickedCommand { get; }


        public FeedbackViewModel(IPopupViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
            OnFeedbackClickedCommand = new Command(async () => await OpenFeedbackDialog());
        }

        private async Task OpenFeedbackDialog(bool isUp = true)
        {
            PopupPage popupPage = _viewFactory.Resolve<FeedbackDialogViewModel>();
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
