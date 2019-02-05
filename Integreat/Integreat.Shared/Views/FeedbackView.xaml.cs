using Autofac;
using Integreat.Shared.ViewModels;

namespace Integreat.Shared.Views
{
    public partial class FeedbackView
    {
        public FeedbackView()
        {
            this.BindingContext = IntegreatApp.Container.Resolve<FeedbackViewModel>();
            InitializeComponent();
        }
    }
}
