using Autofac;
using Integreat.Shared.Utilities;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView
    {
        public FeedbackView()
        {
            this.BindingContext = IntegreatApp.Container.Resolve<FeedbackViewModel>();
            InitializeComponent();
        }

        public static readonly BindableProperty FeedbackTypeProperty = BindableProperty.Create(
                                nameof(FeedbackType),
                                typeof(FeedbackType),
                                typeof(FeedbackView),
                                15);

        public FeedbackType FeedbackType
        {
            get => (FeedbackType)GetValue(FeedbackTypeProperty);
            set => SetValue(FeedbackTypeProperty, value);
        }

    }
}
