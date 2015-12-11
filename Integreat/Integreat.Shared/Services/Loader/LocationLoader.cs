using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services.Loader
{
    public class LocationLoader
    {
        private readonly INetworkService _networkService;
        private readonly PersistenceService _persistenceService;

        public LocationLoader(PersistenceService persistenceService, INetworkService networkService)
        {
            _persistenceService = persistenceService;
            _networkService = networkService;
        }

        public async Task<List<Location>> Load()
        {
            var databaseLocations = await
                _persistenceService.Connection.Table<Location>()
                    .ToListAsync() ?? new List<Location>();
            if (databaseLocations.Count != 0 && Preferences.LastLocationUpdateTime().AddHours(4) >= DateTime.Now)
            {
                return databaseLocations;
            }
            var networkLocations = await _networkService.GetLocations();
            if (networkLocations != null)
            {
                await _persistenceService.InsertAll(networkLocations);
                Preferences.SetLastLocationUpdateTime();
            }
            return await _persistenceService.GetLocations().DefaultIfFaulted(new List<Location>());
        }
    }
}
