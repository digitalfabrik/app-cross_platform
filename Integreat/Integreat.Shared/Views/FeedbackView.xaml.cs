using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Integreat.Shared.Pages.Feedback;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView : ContentView
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
