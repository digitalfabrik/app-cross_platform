using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services.Loader
{
	public class LocationsLoader
	{
		private readonly INetworkService _networkService;
		private readonly PersistenceService _persistenceService;

		public LocationsLoader (PersistenceService persistenceService, INetworkService networkService)
		{
			_persistenceService = persistenceService;
			_networkService = networkService;
		}

		public async Task<List<Location>> Load (bool forceRefresh = false)
		{
            Console.WriteLine("Load is called");
		    var databaseLocations = await _persistenceService.GetLocations();
			if (!forceRefresh && databaseLocations.Count != 0 && Preferences.LastLocationUpdateTime ().AddHours (4) >= DateTime.Now) {
				return databaseLocations;
			}
			var networkLocations = await _networkService.GetLocations ();
			if (networkLocations != null) {
				await _persistenceService.InsertAll (networkLocations);
				Preferences.SetLastLocationUpdateTime ();
			}
			return await _persistenceService.GetLocations ().DefaultIfFaulted (new List<Location> ());
		}
	}
}
