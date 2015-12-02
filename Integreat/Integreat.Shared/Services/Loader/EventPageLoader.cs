using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;

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
            var lastUpdatedPage = await
                _persistenceService.Connection.Table<EventPage>().OrderBy(x => x.Modified.Ticks).FirstOrDefaultAsync();
            var databasePages = await
                _persistenceService.Connection.Table<EventPage>()
                    .Where(x => x.LanguageId == _language.PrimaryKey)
                    .ToListAsync();
            if (databasePages.Count != 0 && lastUpdatedPage.Modified.AddHours(4) >= DateTime.Now)
            {
                return databasePages;
            }
            var networkPages = await _networkService.GetEventPages(_language, _location, new UpdateTime(lastUpdatedPage.Modified.Ticks));
            await _persistenceService.Insert(networkPages);
            return await _persistenceService.GetEventPages(_language);
        } 
    }
}
