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
        private readonly Func<string, FeedbackDialogViewModel> _feedbackDialogViewFactory;
        public ICommand OnFeedbackClickedCommand { get; }


        public FeedbackViewModel(IPopupViewFactory viewFactory, Func<string, FeedbackDialogViewModel> feedbackDialogViewFactory)
        {
            _viewFactory = viewFactory;
            _feedbackDialogViewFactory = feedbackDialogViewFactory;
            OnFeedbackClickedCommand = new Command<string>(OpenFeedbackDialog);
        }

        private async void OpenFeedbackDialog(string value)
        {
            PopupPage popupPage = _viewFactory.Resolve(_feedbackDialogViewFactory(value));
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
