using Autofac;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Newtonsoft.Json;
using Refit;
using Integreat.Shared.Services;

namespace Integreat.ApplicationObject
{
	public class AppSetup
	{
		public IContainer CreateContainer ()
		{
			var containerBuilder = new ContainerBuilder ();
			RegisterDependencies (containerBuilder);
			return containerBuilder.Build ();
		}

		protected virtual void RegisterDependencies (ContainerBuilder cb)
		{
			var networkServiceSettings = new RefitSettings {
				JsonSerializerSettings = new JsonSerializerSettings {
					NullValueHandling = NullValueHandling.Ignore
				}
			};
//			var networkService = RestService.For<INetworkService> ("http://vmkrcmar21.informatik.tu-muenchen.de/", networkServiceSettings);
			var networkService = new NetworkServiceMock ();
			cb.Register (c => new SafeNetworkService (networkService)).As<INetworkService> ();
		}
	}
}
