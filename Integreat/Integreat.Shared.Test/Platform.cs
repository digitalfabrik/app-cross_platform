using Autofac;
using Integreat.ApplicationObject;
#if __ANDROID__
using Integreat.Droid;
#else
using Integreat.iOS;
#endif

namespace Integreat.Shared.Test
{
    public class Platform
    {
        private static ContainerBuilder _containerBuilder;

        public Platform()
        {
            _containerBuilder = new ContainerBuilder();
        }
        public static AppSetup Setup => new AppSetup(new IntegreatApp(_containerBuilder), _containerBuilder);
        public static IContainer Container => _containerBuilder.Build();
    }
}
