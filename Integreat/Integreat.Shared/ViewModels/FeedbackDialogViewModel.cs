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
        public FeedbackDialogViewModel()
        {
            ClosePopupCommand = new Command(ClosePopup);
        }

        public ICommand ClosePopupCommand { get; }

        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
