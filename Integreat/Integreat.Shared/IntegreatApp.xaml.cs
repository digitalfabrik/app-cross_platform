using System.Security;
using Autofac;
using Integreat.ApplicationObject;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;
using Integreat.Utilities;
using Integreat.Shared.Utilities;
using System.Diagnostics;
using Integreat.Shared.Services;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Integreat.Shared
{
    /// <inheritdoc />
    /// <summary>
    /// Application Starting point
    /// </summary>
    [SecurityCritical]
    // ReSharper disable once RedundantExtendsListEntry
    public partial class IntegreatApp : Application
    {
        private readonly AppSetup _app;

        [SecurityCritical]
        public IntegreatApp(ContainerBuilder builder)
        {
            InitializeComponent();
            _app = new AppSetup(this, builder);
            _app.Run();
        }

        //is called when users clicks on a deepLink
        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            const string appDomain = Constants.IntegreatReleaseUrl;
            if (!uri.ToString().ToLower().StartsWith(appDomain, StringComparison.Ordinal) || _app == null) return;

            var deeplinkservice = (DeepLinkService)_app.Container.Resolve<IDeepLinkService>();
            if (deeplinkservice == null) return;
            deeplinkservice.Url = uri;
            try
            {
                deeplinkservice.Navigate();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            base.OnAppLinkRequestReceived(uri);
        }
    }
}
