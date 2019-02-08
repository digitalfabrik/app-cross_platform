using System;
using Autofac;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView
    {
        private readonly IPopupViewFactory _viewFactory;


        public FeedbackView()
        {
            _viewFactory = IntegreatApp.Container.Resolve<IPopupViewFactory>();
            InitializeComponent();
        }

        public static readonly BindableProperty FeedbackTypeProperty = BindableProperty.Create(
                                nameof(FeedbackType),
                                typeof(FeedbackType),
                                typeof(FeedbackView),
                                FeedbackType.Categories);

        public FeedbackType FeedbackType
        {
            get => (FeedbackType)GetValue(FeedbackTypeProperty);
            set => SetValue(FeedbackTypeProperty, value);
        }

        public void OnFeedbackUpClicked(object sender, EventArgs args)
        {
            OpenFeedbackDialog("up");
        }

        public void OnFeedbackDownClicked(object sender, EventArgs args)
        {
            OpenFeedbackDialog("down");
        }

        private async void OpenFeedbackDialog(string value)
        {
            Func<string, FeedbackType, FeedbackDialogViewModel> _feedbackDialogViewFactory = IntegreatApp.Container.Resolve<Func<string, FeedbackType, FeedbackDialogViewModel>>();
            PopupPage popupPage = _viewFactory.Resolve(_feedbackDialogViewFactory(value, FeedbackType));
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
