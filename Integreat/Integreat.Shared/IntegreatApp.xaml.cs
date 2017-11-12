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
            string appDomain = "https://web.integreat-app.de/";
            if (!uri.ToString().ToLowerInvariant().StartsWith(appDomain))
                return;

            string[] segments = uri.Segments;
            if (segments[0].Equals("/"))
            {
                segments = segments.Where(s => s != segments[0]).ToArray();
            }
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].Contains("/"))
                {
                    segments[i] = segments[i].Trim(new Char[] { '/' });
                }
            }

            //webapp url to cms url

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
