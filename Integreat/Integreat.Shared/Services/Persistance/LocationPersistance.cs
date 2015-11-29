using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;

namespace Integreat.Shared.Services.Persistance
{
    public partial class PersistanceService
    {
        public Task<List<Location>> GetLocations()
        {
            var query = Connection.Table<Location>();
            return query.ToListAsync();
        }

    }
}


