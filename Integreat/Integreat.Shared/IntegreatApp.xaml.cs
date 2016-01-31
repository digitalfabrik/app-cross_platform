using Autofac;
using Integreat.ApplicationObject;
using Xamarin.Forms;

namespace Integreat.Shared
{
    public partial class IntegreatApp : Application
    {

        public IntegreatApp(ContainerBuilder builder)
        {
            InitializeComponent();
            var app = new AppSetup(this, builder);
            app.Run();
        }
    }
}
