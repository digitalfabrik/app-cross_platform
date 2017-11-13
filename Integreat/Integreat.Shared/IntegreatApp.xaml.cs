using System.Security;
using Autofac;
using Integreat.ApplicationObject;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;
using System.Linq;
using Integreat.Shared.Converters;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Integreat.Utilities;

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
            string appDomain = Constants.IntegreatReleaseUrl;
            if (!uri.ToString().ToLowerInvariant().StartsWith(appDomain))
                return;

            string[] segments = uri.Segments.Where(s => s != "/").ToArray().Select(s => s.Trim(new Char[] { '/' })).ToArray();

            //webapp url to cms url

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
