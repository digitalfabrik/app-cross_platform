using Autofac;
using Integreat.Shared;
using Integreat.Shared.Services.Tracking;

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
            this.InitializeComponent();

            var cb = new ContainerBuilder();
            cb.RegisterInstance(CreateAnalytics());
            LoadApplication(new IntegreatApp(cb));
        }

        private IAnalyticsService CreateAnalytics()
        {
            var instance = AnalyticsService.GetInstance();
            instance.Initialize(this);
            return instance;
        }

    }
}
