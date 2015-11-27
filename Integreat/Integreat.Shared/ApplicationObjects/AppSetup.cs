using System;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Refit;

namespace Integreat
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
            cb.Register(c => RestService.For<INetworkService>("http://vmkrcmar21.informatik.tu-muenchen.de/")).As<INetworkService>();
            cb.Register(c => new PersistanceService()).As<IPersistanceService>();
            
        }
        
    }
}
