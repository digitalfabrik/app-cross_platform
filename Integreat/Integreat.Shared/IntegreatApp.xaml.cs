using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Utilities;
using System.Security;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Integreat.Shared
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
