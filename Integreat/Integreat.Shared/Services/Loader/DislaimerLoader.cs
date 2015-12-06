using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services.Loader
{
    public class DisclaimerLoader
    {
        private readonly INetworkService _networkService;
        private readonly PersistenceService _persistenceService;
        private readonly Language _language;
        private readonly Location _location;

        public DisclaimerLoader(Language language, Location location, PersistenceService persistenceService, INetworkService networkService)
        {
            _language = language;
            _location = location;
            _persistenceService = persistenceService;
            _networkService = networkService;
        }


        public async Task<List<Disclaimer>> Load()
        {
            var databaseDisclaimers = await
                _persistenceService.Connection.Table<Disclaimer>()
                    .Where(x => x.LanguageId == _language.PrimaryKey)
                    .ToListAsync();
            var lastUpdate = Preferences.LastPageDisclaimerUpdateTime(_language, _location);
            if (databaseDisclaimers.Count != 0 && lastUpdate.AddHours(4) >= DateTime.Now)
            {
                return databaseDisclaimers;
            }
            var networkPages = await _networkService.GetDisclaimers(_language, _location, new UpdateTime(lastUpdate.Ticks));
            var disclaimerIdMappingDictionary = databaseDisclaimers.ToDictionary(disclaimer => disclaimer.Id, disclaimer => disclaimer.PrimaryKey);

            //set language id so that we replace the language and dont add duplicates into the database!
            foreach (var disclaimer in networkPages)
            {
                int networkPagePrimaryKey;
                if (disclaimerIdMappingDictionary.TryGetValue(disclaimer.Id, out networkPagePrimaryKey))
                {
                    disclaimer.PrimaryKey = networkPagePrimaryKey;
                }
                disclaimer.LanguageId = _language.PrimaryKey;
            }
            await _persistenceService.InsertAll(networkPages);
            Preferences.SetLastPageDisclaimerUpdateTime(_language, _location);
            return await _persistenceService.GetDisclaimers(_language);
        }
    }
}
