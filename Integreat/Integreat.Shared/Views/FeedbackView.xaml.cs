using System;
using Autofac;
using Integreat.Shared.Pages.Feedback;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

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
