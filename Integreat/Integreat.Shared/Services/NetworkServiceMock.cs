using System;
using Integreat.Services;
using System.Threading.Tasks;
using Integreat.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using Integreat.Shared.Models;

namespace Integreat.Shared.Test
{
	public class NetworkServiceMock : INetworkService
	{
		Task<string> IsServerAlive ()
			=> Task.Factory.StartNew (() => "some string");

		Task<Collection<Disclaimer>> GetDisclaimers (Language language, Location location, UpdateTime time);

		Task<Collection<Page>> GetPages (Language language, Location location, UpdateTime time);

		Task<HttpResponseMessage> GetPagesDebug (Language language, Location location, UpdateTime time);

		Task<Collection<EventPage>> GetEventPages (Language language, Location location, UpdateTime time);

		Task<Collection<Location>> GetLocations ();

		Task<Collection<Language>> GetLanguages (Location location);

		Task<string> SubscribePush (Location location, string regId);

		Task<string> UnsubscribePush (Location location, string regId);
	}
}

