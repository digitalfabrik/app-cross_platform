using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fusillade;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Utilities;
using Plugin.Connectivity;
using Polly;

namespace Integreat.Shared.Services.Loader
{
    public class LocationsLoader
    {
        private readonly INetworkService _networkService;
        private readonly PersistenceService _persistenceService;

        public LocationsLoader(PersistenceService persistenceService,
            Func<Priority, INetworkService> networkServiceFactory, Priority priority = Priority.Background)
        {
            _persistenceService = persistenceService;
            _networkService = networkServiceFactory(priority);
        }

        public async Task<List<Location>> Load(bool forceRefresh = false)
        {
            Console.WriteLine("Load is called");
            var databaseLocations = await _persistenceService.GetLocations();
            if (!forceRefresh && databaseLocations.Count != 0 &&
                Preferences.LastLocationUpdateTime().AddHours(4) >= DateTime.Now)
            {
                return databaseLocations;
            }

            var networkLocations = CrossConnectivity.Current.IsConnected
                ? await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(
                        async () =>
                            await _networkService.GetLocations())
                : null;
            if (networkLocations != null)
            {
                await _persistenceService.InsertAll(networkLocations);
                Preferences.SetLastLocationUpdateTime();
            }
            return await _persistenceService.GetLocations().DefaultIfFaulted(new List<Location>());
        }
    }
}
