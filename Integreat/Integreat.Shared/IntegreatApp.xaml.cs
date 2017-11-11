using System.Security;
using Autofac;
using Integreat.ApplicationObject;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Integreat.Shared
{
    [SecurityCritical]
    public partial class IntegreatApp : Application
    {
        [SecurityCritical]
        public IntegreatApp(ContainerBuilder builder)
        {
            InitializeComponent();
            var app = new AppSetup(this, builder);
            app.Run();
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
        }
    }
}
