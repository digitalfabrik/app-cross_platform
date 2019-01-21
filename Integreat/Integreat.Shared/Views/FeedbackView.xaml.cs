using System;
using Integreat.Shared.Pages.Feedback;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView
    {
        private readonly IPopupViewFactory _viewFactory;

        public FeedbackView(IPopupViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
            InitializeComponent();
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
            PopupPage popupPage = _viewFactory.Resolve<FeedbackDialogViewModel>();
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
