using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using SQLite.Net.Async;

namespace Integreat.Shared.Services.Persistence
{
    public partial class PersistenceService
    {
        public Task<List<T>> GetPages<T>(Language language, string parentPage) where T : Page
        {
            AsyncTableQuery<T> query = null;
            if (parentPage == null)
            {
                query = Connection.Table<T>().Where(
                x => x.LanguageId == language.PrimaryKey &&
                !"trash".Equals(x.Status));
            }
            else {
                query = Connection.Table<T>().Where(
                   x => x.LanguageId == language.PrimaryKey &&
                   !"trash".Equals(x.Status) &&
                   // if a parent-page is set, we only return pages with this parent-id
                   x.ParentId == parentPage);
            }
            return query.ToListAsync().DefaultIfFaulted(new List<T>());
        }
    }
}
