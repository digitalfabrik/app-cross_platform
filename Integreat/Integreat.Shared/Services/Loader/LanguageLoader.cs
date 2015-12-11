using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services.Loader
{
    public class LanguageLoader
    {
        private readonly INetworkService _networkService;
        private readonly PersistenceService _persistenceService;
        private readonly Location _location;

        public LanguageLoader(Location location, PersistenceService persistenceService, INetworkService networkService)
        {
            _location = location;
            _persistenceService = persistenceService;
            _networkService = networkService;
        }

        public async Task<List<Language>> Load()
        {
            var databaseLanguages = await
                _persistenceService.Connection.Table<Language>()
                    .Where(x => x.LocationId == _location.Id)
                    .ToListAsync() ?? new List<Language>();
            if (databaseLanguages.Count != 0 &&
                Preferences.LastLanguageUpdateTime(_location).AddHours(4) >= DateTime.Now)
            {
                return databaseLanguages;
            }
            var networkLanguages = await _networkService.GetLanguages(_location);
            if (networkLanguages != null)
            {
                var languageIdMappingDictionary = databaseLanguages.ToDictionary(language => language.Id,
                    language => language.PrimaryKey);

                //set language id so that we replace the language and dont add duplicates into the database!
                foreach (var language in networkLanguages)
                {
                    int languagePrimaryKey;
                    if (languageIdMappingDictionary.TryGetValue(language.Id, out languagePrimaryKey))
                    {
                        language.PrimaryKey = languagePrimaryKey;
                    }
                    language.LocationId = _location.Id;
                    language.Location = _location;
                }
                await _persistenceService.InsertAll(networkLanguages);
                Preferences.SetLastLanguageUpdateTime(_location);
            }
            return await _persistenceService.GetLanguages(_location).DefaultIfFaulted(new List<Language>());
        }
    }
}
