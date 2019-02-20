using System.Security;
using App1.ApplicationObjects;
using App1.Utilities;
using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App1
{
    /// <inheritdoc />
    [SecurityCritical]
    public partial class IntegreatApp
    {
        [SecurityCritical]
        public IntegreatApp(ContainerBuilder builder)
        {
            InitializeComponent();
            var app = new AppSetup(this, builder);
            app.Run();
        }

        public static IContainer Container;

        private static ILogger _logger;
        public static ILogger Logger => _logger ?? (_logger = DependencyService.Get<ILogger>());
    }
}
