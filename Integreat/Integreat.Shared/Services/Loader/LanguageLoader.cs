using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;

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
            var lastUpdatedPage = await
                _persistenceService.Connection.Table<Language>().OrderBy(x => x.Modified.Ticks).FirstOrDefaultAsync();
            var databasePages = await
                _persistenceService.Connection.Table<Language>()
                    .Where(x => x.LocationId == _location.Id)
                    .ToListAsync();
            if (databasePages.Count != 0 && lastUpdatedPage.Modified.AddHours(4) >= DateTime.Now)
            {
                return databasePages;
            }
            var networkLanguages = await _networkService.GetLanguages(_location);
            await _persistenceService.Insert(networkLanguages);
            return await _persistenceService.GetLanguages(_location);
        } 
    }
}
