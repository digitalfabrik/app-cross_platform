using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Shared.Models;
using Refit;

namespace Integreat.Services
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
            var task = _networkService.IsServerAlive();
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<Collection<Disclaimer>> GetDisclaimers(Language language, Location location, UpdateTime time)
        {
            var task = _networkService.GetDisclaimers(language, location, time).ContinueWith(t => default(Collection<Disclaimer>), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<Collection<Page>> GetPages(Language language, Location location, UpdateTime time)
        {
            var task = _networkService.GetPages(language, location, time);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<HttpResponseMessage> GetPagesDebug(Language language, Location location, UpdateTime time)
        {
            var task = _networkService.GetPagesDebug(language, location, time).ContinueWith(t => default(HttpResponseMessage), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<Collection<EventPage>> GetEventPages(Language language, Location location, UpdateTime time)
        {
            var task = _networkService.GetEventPages(language, location, time).ContinueWith(t => default(Collection<EventPage>), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<Collection<Location>> GetLocations()
        {
            var task = _networkService.GetLocations().ContinueWith(t => default(Collection<Location>), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<Collection<Language>> GetLanguages(Location location)
        {
            var task = _networkService.GetLanguages(location).ContinueWith(t => default(Collection<Language>), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<string> SubscribePush(Location location, string regId)
        {
            var task = _networkService.SubscribePush(location, regId).ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        public Task<string> UnsubscribePush(Location location, string regId)
        {
            var task = _networkService.UnsubscribePush(location, regId).ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => default(string), TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }
    }
}

