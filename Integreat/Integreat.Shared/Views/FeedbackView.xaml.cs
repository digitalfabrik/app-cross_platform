using Autofac;
using Integreat.Localization;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
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

            FeedbackLabel.Text = AppResources.Feedback;
        }

        public static readonly BindableProperty FeedbackTypeProperty = BindableProperty.Create(
                                nameof(FeedbackType),
                                typeof(FeedbackType),
                                typeof(FeedbackView),
                                FeedbackType.Categories);

        public static readonly BindableProperty PageIdProperty = BindableProperty.Create(
                                nameof(PageId),
                                typeof(int?),
                                typeof(FeedbackView));

        public static readonly BindableProperty ExtraStringProperty = BindableProperty.Create(
                        nameof(ExtraString),
                        typeof(string),
                        typeof(FeedbackView));

        //should be nullable
        public int? PageId
        {
            get => (int?)GetValue(PageIdProperty);
            set => SetValue(PageIdProperty, value);
        }

        //for alias, permalink, query
        public string ExtraString
        {
            get => (string)GetValue(ExtraStringProperty);
            set => SetValue(ExtraStringProperty, value);
        }

        public FeedbackType FeedbackType
        {
            get => (FeedbackType)GetValue(FeedbackTypeProperty);
            set => SetValue(FeedbackTypeProperty, value);
        }

        public void OnFeedbackUpClicked(object sender, EventArgs args)
        {
            OpenFeedbackDialog(FeedbackKind.Up);
        }

        public void OnFeedbackDownClicked(object sender, EventArgs args)
        {
            OpenFeedbackDialog(FeedbackKind.Down);
        }

        private async void OpenFeedbackDialog(FeedbackKind feedbackKind)
        {
            var feedbackDialogViewFactory = IntegreatApp.Container.Resolve<Func<FeedbackKind, FeedbackType, int?, string, FeedbackDialogViewModel>>();
            var popupPage = _viewFactory.Resolve(feedbackDialogViewFactory(feedbackKind, FeedbackType, PageId, ExtraString));
            await PopupNavigation.Instance.PushAsync(popupPage);
        }
    }
}
