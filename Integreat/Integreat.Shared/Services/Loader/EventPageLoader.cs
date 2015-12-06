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
    public class EventPageLoader
    {
        private readonly INetworkService _networkService;
        private readonly PersistenceService _persistenceService;
        private readonly Language _language;
        private readonly Location _location;

        public EventPageLoader(Language language, Location location, PersistenceService persistenceService, INetworkService networkService)
        {
            _language = language;
            _location = location;
            _persistenceService = persistenceService;
            _networkService = networkService;
        }


        public async Task<List<EventPage>> Load()
        {
            var databasePages = await
                _persistenceService.Connection.Table<EventPage>()
                    .Where(x => x.LanguageId == _language.PrimaryKey)
                    .ToListAsync();
            var lastUpdate = Preferences.LastEventPageUpdateTime(_language, _location);
            if (databasePages.Count != 0 && lastUpdate.AddHours(4) >= DateTime.Now)
            {
                return databasePages;
            }
            var networkPages = await _networkService.GetEventPages(_language, _location, new UpdateTime(lastUpdate.Ticks));
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
            Preferences.SetLastEventPageUpdateTime(_language, _location);
            return await _persistenceService.GetEventPages(_language);
        }
    }
}
