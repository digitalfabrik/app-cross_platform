using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fusillade;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Services.Persistence;

namespace Integreat.Shared.Services.Loader
{
	public class DisclaimerLoader : AbstractPageLoader<Disclaimer>
	{
		public DisclaimerLoader (Language language, Location location, PersistenceService persistenceService, Func<Priority, INetworkService> networkServiceFactory, Priority priority = Priority.Background)
			: base (language, location, persistenceService, networkServiceFactory, priority)
		{
		}

        public override Task<Collection<Disclaimer>> LoadNetworkPages (UpdateTime time)
		{
			return NetworkService.GetDisclaimers (Language, Location, time);
		}
	}
}
