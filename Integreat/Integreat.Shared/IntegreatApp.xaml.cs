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

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            const string appDomain = Constants.IntegreatReleaseUrl;
            if (!uri.ToString().ToLower().StartsWith(appDomain, StringComparison.Ordinal) || _app == null) return;

            var deeplinkservice = (DeepLinkService)_app.Container.Resolve<IDeepLinkService>();
            if (deeplinkservice == null) return;
            deeplinkservice.Url = uri;
            try
            {
                deeplinkservice.Navigate(_app.Container.Resolve<IShortnameParser>());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            base.OnAppLinkRequestReceived(uri);
        }
    }
}
