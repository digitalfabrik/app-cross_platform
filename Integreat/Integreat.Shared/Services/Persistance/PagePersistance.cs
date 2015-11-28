using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;

namespace Integreat.Shared.Services.Persistance
{
	public partial class PersistanceService {

		public Task<List<Page>> GetPages(Language language, Location location)
        {
            var query = _database.Table<Page>().Where(x => x.Language.Id == language.Id
                            && x.Language.Location.Id == location.Id);
		    return query.ToListAsync();
        }

	    public Task<Page> GetPage(int id)
	    {
	        return _database.Table<Page>().Where(x => x.Id == id).FirstAsync();
	    }
		
	}
}


