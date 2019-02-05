using System;
using System.Windows.Input;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FeedbackDialogViewModel : BaseViewModel
    {

        private readonly string _kindOfFeedback;

        public FeedbackDialogViewModel(string kindOfFeedback)
        {
            _kindOfFeedback = kindOfFeedback;
            ClosePopupCommand = new Command(ClosePopup);
            SendFeedbackCommand = new Command(SendFeedback);
        }

        public ICommand ClosePopupCommand { get; }
        public ICommand SendFeedbackCommand { get; }

        public async void SendFeedback()
        {
            System.Diagnostics.Debug.WriteLine("Kind of feedback: " + _kindOfFeedback);
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
