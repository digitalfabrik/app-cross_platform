using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Integreat.Shared
{
    /// <inheritdoc />
    public partial class IntegreatApp
    {
        public IntegreatApp(ContainerBuilder builder)
        {
            InitializeComponent();
            var app = new AppSetup(this, builder);
            app.Run();
        }

        private static ILogger _logger;
        public static ILogger Logger => _logger ?? (_logger = DependencyService.Get<ILogger>());

        public static IContainer Container { get; set; }
    }
}
