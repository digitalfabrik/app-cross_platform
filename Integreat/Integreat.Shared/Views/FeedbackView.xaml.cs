using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
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

        private void OpenFeedbackDialog(bool isUp) {

        }


    }
}
