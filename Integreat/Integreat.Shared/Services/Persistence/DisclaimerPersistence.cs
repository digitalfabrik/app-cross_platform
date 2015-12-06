using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Shared.Models;

namespace Integreat.Shared.Services.Persistance
{
	public partial class PersistenceService {

		public Task<List<Disclaimer>> GetDisclaimers(Language language)
        {
            var query = Connection.Table<Disclaimer>().Where(x => x.LanguageId == language.PrimaryKey);
		    return query.ToListAsync();
        }
		
	}
}


