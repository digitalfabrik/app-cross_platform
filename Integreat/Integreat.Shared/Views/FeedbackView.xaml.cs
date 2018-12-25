using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView : ContentView
    {
        public FeedbackView()
        {
            InitializeComponent();
            OpenFeedbackDialogCommand = new Command(OpenFeedbackDialog);
        }

        public ICommand OpenFeedbackDialogCommand { get; }

        private void OpenFeedbackDialog(object s) {
            
        }
    }
}
