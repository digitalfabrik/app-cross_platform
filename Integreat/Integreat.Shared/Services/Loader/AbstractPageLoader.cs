using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fusillade;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Utilities;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Polly;

namespace Integreat.Shared.Services.Loader
{
    public abstract class AbstractPageLoader<T> where T : Page
    {
        private const int NoReloadTimeout = 4;
        protected INetworkService NetworkService { get; }
        private readonly PersistenceService _persistenceService;
        protected Language Language;
        protected Location Location;

        protected AbstractPageLoader(Language language, Location location, PersistenceService persistenceService,
            Func<Priority, INetworkService> networkServiceFactory, Priority priority = Priority.Background)
        {
            if (language == null)
            {
                throw new ArgumentNullException("language");
            }
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }
            Language = language;
            Location = location;
            _persistenceService = persistenceService;
            NetworkService = networkServiceFactory(priority);
        }

        public abstract Task<Collection<T>> LoadNetworkPages(UpdateTime time);

        public async Task<List<T>> Load(bool forceRefresh = false, string parentPage = null)
        {
            var databasePages = await _persistenceService.GetPages<T>(Language, parentPage) ?? new List<T>();
            Console.WriteLine("Database Pages received: " + databasePages.Count);

            var lastUpdate = Preferences.LastPageUpdateTime<T>(Language, Location);
            // if we did not force a refresh, and the last update is not that far away and the database is not empty, we return the database-values
            if (!forceRefresh && databasePages.Count != 0 && lastUpdate.AddHours(NoReloadTimeout) >= DateTime.Now)
            {
                return databasePages;
            }
            // if database is empty, do a full scan and not only from the latest update

            //if true, the current connection uses cellular connection (lte, 3g, ...)
            var cellularUsage = CrossConnectivity.Current.ConnectionTypes.Any(x => x == ConnectionType.Cellular);

            //either connection is not restricted to WiFi or the user is not connected to cellular networks
            //we should allow the user to refresh, even though the network type might not be allowed
            var allowedToUseNetworkConection = forceRefresh || Preferences.ConnectionType != ConnectionType.WiFi ||
                                               !cellularUsage;

            var networkPages = CrossConnectivity.Current.IsConnected && allowedToUseNetworkConection
                ? await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .ExecuteAsync(
                        async () =>
                            await LoadNetworkPages(new UpdateTime(databasePages.Count == 0 ? 0 : lastUpdate.Ticks)))
                : null;

            if (networkPages == null)
            {
                return await _persistenceService.GetPages<T>(Language, parentPage).DefaultIfFaulted(new List<T>());
            }
            foreach(var page in networkPages)
            {
                page.PrimaryKey = page.Id + "_" + Language.Id + "_" + Location.Id;
                page.LanguageId = Language.PrimaryKey;
                if (page.ParentJsonId > 0)
                {
                    page.ParentId = page.ParentJsonId + "_" + Language.Id + "_" + Location.Id;
                }
                page.AvailableLanguages?.ForEach(x => x.OwnPageId = page.PrimaryKey);
            }
            
            await _persistenceService.InsertAll(networkPages);
            Preferences.SetLastPageUpdateTime<T>(Language, Location);
            return await _persistenceService.GetPages<T>(Language, parentPage).DefaultIfFaulted(new List<T>());
        }
    }
}
