using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;

namespace Integreat.Shared.Services.Persistance
{
    public partial class PersistanceService
    {
        public Task<List<EventPage>> GetEventPages(Language language, Location location)
        {
            var query = _database.Table<EventPage>().Where(x => x.Language.Id == language.Id
                                                           && x.Language.Location.Id == location.Id);
            return query.ToListAsync();
        }

        public Task<EventPage> GetEventPage(int id)
        {
            return _database.Table<EventPage>().Where(x => x.Id == id).FirstAsync();
        }

    }
}
