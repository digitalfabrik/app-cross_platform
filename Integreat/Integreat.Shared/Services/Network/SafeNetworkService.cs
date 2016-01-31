using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Services.Network
{
    // This class is a 1:1 android-networkservice equivalent using RetFit instead of Retrofit
    public class SafeNetworkService : INetworkService
    {
        private readonly INetworkService _networkService;

        public SafeNetworkService(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public Task<string> IsServerAlive()
        {
            return _networkService.IsServerAlive().DefaultIfFaulted("");
        }

        public Task<Collection<Disclaimer>> GetDisclaimers(Language language, Location location, UpdateTime time)
        {
            return _networkService.GetDisclaimers(language, location, time).DefaultIfFaulted(new Collection<Disclaimer>());
        }

        public Task<Collection<Page>> GetPages(Language language, Location location, UpdateTime time)
        {
            return _networkService.GetPages(language, location, time).DefaultIfFaulted(new Collection<Page>());
        }

        public Task<HttpResponseMessage> GetPagesDebug(Language language, Location location, UpdateTime time)
        {
            return _networkService.GetPagesDebug(language, location, time).DefaultIfFaulted();
        }

        public Task<Collection<EventPage>> GetEventPages(Language language, Location location, UpdateTime time)
        {
            return _networkService.GetEventPages(language, location, time).DefaultIfFaulted(new Collection<EventPage>());
        }

        public Task<Collection<Location>> GetLocations()
        {
            return _networkService.GetLocations().DefaultIfFaulted();
        }

        public Task<Collection<Language>> GetLanguages(Location location)
        {
            return _networkService.GetLanguages(location).DefaultIfFaulted(new Collection<Language>());
        }

        public Task<string> SubscribePush(Location location, string regId)
        {
            return _networkService.SubscribePush(location, regId).DefaultIfFaulted();
        }

        public Task<string> UnsubscribePush(Location location, string regId)
        {
            return _networkService.UnsubscribePush(location, regId).DefaultIfFaulted();
        }
    }
}
