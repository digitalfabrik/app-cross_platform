using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;
using SQLiteNetExtensionsAsync.Extensions;

namespace Integreat.Shared.Services.Persistance
{
    public partial class PersistanceService
    {
        public Task<List<Language>> GetLanguages(Location location)
        {
            var query = _database.Table<Language>().Where(x => x.Location.Id == location.Id);
            return query.ToListAsync();
        }
    }
}


