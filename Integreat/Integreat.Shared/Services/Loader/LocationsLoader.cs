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
            var locationsCount = await _persistenceService.GetLocationsCount();
            if (!forceRefresh && locationsCount != 0 &&
                Preferences.LastLocationUpdateTime().AddHours(4) >= DateTime.Now)
            {
                return await _persistenceService.GetLocations().DefaultIfFaulted(new List<Location>());
            }

            var networkLocations = CrossConnectivity.Current.IsConnected
                ? await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(
                        async () =>
                            await _networkService.GetLocations())
                : null;
            if (networkLocations.IsNullOrEmpty())
            {
                return locationsCount == 0 ? new List<Location>() : await _persistenceService.GetLocations().DefaultIfFaulted(new List<Location>());
            }
                await _persistenceService.InsertAll(networkLocations);
                Preferences.SetLastLocationUpdateTime(DateTime.Now);
            return await _persistenceService.GetLocations().DefaultIfFaulted(new List<Location>());
        }
    }
}
