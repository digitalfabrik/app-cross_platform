using Integreat.Shared.Models;
using Integreat.Services;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Persistance;
using NUnit.Framework;

namespace Integreat.Shared.Test.Services.Loader
{
	[TestFixture]
	internal class EventPageLoaderTest : AbstractPageLoaderTest<EventPage>
	{
		public override AbstractPageLoader<EventPage> GetPageLoader (Language language, Location location, PersistenceService persistenceService,
		                                                                  INetworkService networkService)
		{
			return new EventPageLoader (language, location, persistenceService, networkService);
		}
	}
}