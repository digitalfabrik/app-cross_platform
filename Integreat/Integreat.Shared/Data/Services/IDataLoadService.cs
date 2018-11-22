using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras;
using Refit;

namespace Integreat.Shared.Data
{
    [Headers("Accept: application/json")]
    public interface IDataLoadService
    {  
        [Get("/wp-json/")]
        Task<string> IsServerAlive();

        [Get("/{location}/{language}/wp-json/extensions/v3/disclaimer")]
        Task<Collection<Disclaimer>> GetDisclaimers([AliasAs("language")] Language language, [AliasAs("location")] Location location);

        [Get("/{location}/{language}/wp-json/extensions/v3/pages")]
        Task<Collection<Page>> GetPages([AliasAs("language")] Language language, [AliasAs("location")] Location location);

        [Get("/{location}/{language}/wp-json/extensions/v3/events")]
        Task<Collection<EventPage>> GetEventPages([AliasAs("language")] Language language, [AliasAs("location")] Location location);

        [Get("/wp-json/extensions/v3/sites/")]
        Task<Collection<Location>> GetLocations();

        [Get("/{location}/{language}/wp-json/extensions/v3/extras/")]
        Task<Collection<Extra>> GetExtras([AliasAs("language")] Language language, [AliasAs("location")] Location location);

        [Get("/{location}/de/wp-json/extensions/v3/languages")]
        Task<Collection<Language>> GetLanguages([AliasAs("location")] Location location);
    }
}
