using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;

namespace Integreat.Shared.Services.Persistance
{
	public partial class PersistenceService {

		public Task<List<T>> GetPages<T>(Language language) where T : Page
        {
            var query = Connection.Table<T>().Where(x => x.LanguageId == language.PrimaryKey && !"trash".Equals(x.Status));
		    return query.ToListAsync();
        }
		
	}
}


