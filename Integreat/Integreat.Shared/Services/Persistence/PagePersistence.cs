using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;

namespace Integreat.Shared.Services.Persistance
{
	public partial class PersistenceService {

		public Task<List<Page>> GetPages(Language language)
        {
            var query = Connection.Table<Page>().Where(x => x.LanguageId == language.Id);
		    return query.ToListAsync();
        }
		
	}
}


