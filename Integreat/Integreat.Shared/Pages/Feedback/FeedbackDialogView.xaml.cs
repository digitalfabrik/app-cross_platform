using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Feedback
{
    public partial class FeedbackDialogView : Rg.Plugins.Popup.Pages.PopupPage
    { 
        public FeedbackDialogView()
        {
            InitializeComponent();
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
