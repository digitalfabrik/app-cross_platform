using Autofac;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Newtonsoft.Json;
using Refit;

namespace Integreat.ApplicationObject
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            cb.Register(c => RestService.For<INetworkService>("http://vmkrcmar21.informatik.tu-muenchen.de/", new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            })).As<INetworkService>(); 
        }
        
    }
}
