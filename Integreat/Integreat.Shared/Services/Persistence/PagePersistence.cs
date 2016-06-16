using System.Collections.Generic;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using SQLiteNetExtensionsAsync.Extensions;

namespace Integreat.Shared.Services.Persistence
{
    public partial class PersistenceService
    {
        public Task<List<T>> GetPages<T>(Language language, string parentPage) where T : Page
        {
            if (parentPage == null)
            {
                return Connection.GetAllWithChildrenAsync<T>(x => x.LanguageId == language.PrimaryKey &&
                                                                  !"trash".Equals(x.Status))
                    .DefaultIfFaulted(new List<T>());
            }
            return Connection.GetAllWithChildrenAsync<T>(x => x.LanguageId == language.PrimaryKey &&
                                                              !"trash".Equals(x.Status) &&
                                                              // if a parent-page is set, we only return pages with this parent-id
                                                              x.ParentId == parentPage).DefaultIfFaulted(new List<T>());
        }
    }
}
