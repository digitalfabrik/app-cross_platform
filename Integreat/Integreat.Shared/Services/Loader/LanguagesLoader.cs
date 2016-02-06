using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusillade;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services.Loader
{
	public class LanguagesLoader
	{
		private readonly INetworkService _networkService;
		private readonly PersistenceService _persistenceService;
		private readonly Location _location;

		public LanguagesLoader (Location location, PersistenceService persistenceService, Func<Priority, INetworkService> networkServiceFactory, Priority priority = Priority.Background)
		{
			_location = location;
			_persistenceService = persistenceService;
			_networkService = networkServiceFactory(priority);
		}

		public async Task<List<Language>> Load (bool forceRefresh = false)
		{
			var databaseLanguages = await _persistenceService.GetLanguages(_location);
			if (!forceRefresh && databaseLanguages.Count != 0 &&
			             Preferences.LastLanguageUpdateTime (_location).AddHours (4) >= DateTime.Now) {
				return databaseLanguages;
			}
			var networkLanguages = await _networkService.GetLanguages (_location);
			if (networkLanguages != null) {
				var languageIdMappingDictionary = databaseLanguages.ToDictionary (language => language.Id,
					                                              language => language.PrimaryKey);

				//set language id so that we replace the language and dont add duplicates into the database!
				foreach (var language in networkLanguages) {
					int languagePrimaryKey;
					if (languageIdMappingDictionary.TryGetValue (language.Id, out languagePrimaryKey)) {
						language.PrimaryKey = languagePrimaryKey;
					}
					language.LocationId = _location.Id;
					language.Location = _location;
				}
				await _persistenceService.InsertAll (networkLanguages);
				Preferences.SetLastLanguageUpdateTime (_location);
			}
			return await _persistenceService.GetLanguages (_location).DefaultIfFaulted (new List<Language> ());
		}
	}
}
