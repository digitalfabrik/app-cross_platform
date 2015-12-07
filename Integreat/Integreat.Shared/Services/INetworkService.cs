﻿using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Integreat.Models;
using Integreat.Shared.Models;
using Refit;

namespace Integreat.Services
{
    // This class is a 1:1 android-networkservice equivalent using RetFit instead of Retrofit
    public interface INetworkService
    {
        [Get("/wordpress/wp-json/")]
        Task<string> IsServerAlive();

        [Get("/{location}/{language}/wp-json/extensions/v0/modified_content/disclaimer?since={since}")]
        Task<Collection<Disclaimer>> GetDisclaimers([AliasAs("language")] Language language,
            [AliasAs("location")] Location location, [AliasAs("since")] UpdateTime time);

        [Get("/{location}/{language}/wp-json/extensions/v0/modified_content/Pages?since={since}")]
        Task<Collection<Page>> GetPages([AliasAs("language")] Language language, [AliasAs("location")] Location location,
            [AliasAs("since")] UpdateTime time);

        [Get("/{location}/{language}/wp-json/extensions/v0/modified_content/Pages?since={since}")]
        Task<HttpResponseMessage> GetPagesDebug([AliasAs("language")] Language language,
            [AliasAs("location")] Location location, [AliasAs("since")] UpdateTime time);

        [Get("/{location}/{language}/wp-json/extensions/v0/modified_content/events?since={since}")]
        Task<Collection<EventPage>> GetEventPages([AliasAs("language")] Language language,
            [AliasAs("location")] Location location, [AliasAs("since")] UpdateTime time);

        [Get("/wordpress/wp-json/extensions/v0/multisites/")]
        Task<Collection<Location>> GetLocations();

        [Get("/{location}/de/wp-json/extensions/v0/languages/wpml")]
        Task<Collection<Language>> GetLanguages([AliasAs("location")] Location location);

        [Get("/{location}")]
        Task<string> SubscribePush([AliasAs("location")] Location location, [AliasAs("gcm_register_id")] string regId);

        [Get("/{location}")]
        Task<string> UnsubscribePush([AliasAs("location")] Location location,
            [AliasAs("gcm_unregister_id")] string regId);
    }
}

