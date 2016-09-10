using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Services.Persistence
{
	public partial class PersistenceService
	{
		public Task<List<Location>> GetLocations ()
		{
			var query = Connection.Table<Location> ();
			return query.ToListAsync ().DefaultIfFaulted (new List<Location> ());
		}
        public Task<int> GetLocationsCount()
        {
            return Connection.Table<Location>().CountAsync().DefaultIfFaulted();
        }
    }
}


