using Autofac;
using Integreat.Shared;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Integreat.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            var cb = new ContainerBuilder();
            LoadApplication(new IntegreatApp(cb));
        }
    }
}
