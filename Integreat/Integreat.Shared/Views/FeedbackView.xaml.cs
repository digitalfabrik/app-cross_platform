using System;
using Integreat.Shared.Pages.Feedback;
using Rg.Plugins.Popup.Services;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView
    {
        public FeedbackView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnSmileClicked(object sender, EventArgs args)
        {
            OpenFeedbackDialog(true);
        }

        private void OnFrownClicked(object sender, EventArgs args)
        {
            OpenFeedbackDialog(false);
        }

        private async void OpenFeedbackDialog(bool isUp)
        {
            await PopupNavigation.Instance.PushAsync(new FeedbackDialogView());
        }
    }
}
