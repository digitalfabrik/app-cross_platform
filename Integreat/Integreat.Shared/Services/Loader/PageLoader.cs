using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services.Loader
{
    public class PageLoader
    {
        private readonly INetworkService _networkService;
        private readonly PersistenceService _persistenceService;
        private readonly Language _language;
        private readonly Location _location;

        public PageLoader(Language language, Location location, PersistenceService persistenceService, INetworkService networkService)
        {
            _language = language;
            _location = location;
            _persistenceService = persistenceService;
            _networkService = networkService;
        }


        public async Task<List<Page>> Load()
        {
            var databasePages = await
                _persistenceService.Connection.Table<Page>()
                    .Where(x => x.LanguageId == _language.PrimaryKey)
                    .ToListAsync();
            var lastUpdate = Preferences.LastPageUpdateTime(_language, _location);
            if (databasePages.Count != 0 && lastUpdate.AddHours(4) >= DateTime.Now)
            {
                return databasePages;
            }
            var networkPages = await _networkService.GetPages(_language, _location, new UpdateTime(lastUpdate.Ticks));
            var pagesIdMappingDictionary = databasePages.ToDictionary(page => page.Id, page => page.PrimaryKey);

            //set language id so that we replace the language and dont add duplicates into the database!
            foreach (var page in networkPages)
            {
                int networkPagePrimaryKey;
                if (pagesIdMappingDictionary.TryGetValue(page.Id, out networkPagePrimaryKey))
                {
                    page.PrimaryKey = networkPagePrimaryKey;
                }
                page.LanguageId = _language.PrimaryKey;
            }
            await _persistenceService.InsertAll(networkPages);
            Preferences.SetLastPageUpdateTime(_language, _location);
            return await _persistenceService.GetPages(_language);
        }
    }
}
