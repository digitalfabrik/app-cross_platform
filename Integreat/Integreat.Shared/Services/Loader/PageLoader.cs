using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Services;
using Integreat.Shared.Services.Persistance;

namespace Integreat.Shared.Services.Loader
{
    public class PageLoader : AbstractPageLoader<Page>
    {
        public PageLoader(Language language, Location location, PersistenceService persistenceService, INetworkService networkService)
        : base(language, location, persistenceService, networkService)
        {
        }
        
        public override Task<Collection<Page>> LoadNetworkPages(UpdateTime time)
        {
            return NetworkService.GetPages(Language, Location, time);
        }
    }
}
