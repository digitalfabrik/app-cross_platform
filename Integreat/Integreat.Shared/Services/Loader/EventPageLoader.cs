using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Services.Persistence;

namespace Integreat.Shared.Services.Loader
{
	public class EventPageLoader : AbstractPageLoader<EventPage>
	{
		public EventPageLoader (Language language, Location location, PersistenceService persistenceService, INetworkService networkService)
			: base (language, location, persistenceService, networkService)
		{
		}

		public override Task<Collection<EventPage>> LoadNetworkPages (UpdateTime time)
		{
			return NetworkService.GetEventPages (Language, Location, time);
		}
	}
}
