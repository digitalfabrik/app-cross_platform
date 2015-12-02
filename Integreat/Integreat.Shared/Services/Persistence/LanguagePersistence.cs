using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;

namespace Integreat.Shared.Services.Persistance
{
    public partial class PersistenceService
    {
        public Task<List<Language>> GetLanguages(Location location)
        {
            var query = Connection.Table<Language>().Where(x => x.Location.Id == location.Id);
            return query.ToListAsync();
        }
    }
}


