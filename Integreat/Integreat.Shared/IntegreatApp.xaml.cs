using System.Security;
using Autofac;
using Integreat.ApplicationObject;
using Integreat.Shared.Firebase;
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
    }
}
