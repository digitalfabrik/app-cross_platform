using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Fusillade;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Network;
using Integreat.Shared.Services.Persistence;
using Integreat.Shared.Utilities;
using Plugin.Connectivity;
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

		protected AbstractPageLoader (Language language, Location location, PersistenceService persistenceService,
                                      Func<Priority, INetworkService> networkServiceFactory, Priority priority = Priority.Background)
		{
			Language = language;
			Location = location;
			_persistenceService = persistenceService;
            NetworkService = networkServiceFactory(priority);
		}

		public abstract Task<Collection<T>> LoadNetworkPages (UpdateTime time);

	    public async Task<List<T>> Load(bool forceRefresh = false)
	    {
	        if (Language == null || Location == null)
	        {
	            return null; //TODO we need to handle this better :)
	        }
	        var databasePages = await _persistenceService.GetPages<T>(Language) ?? new List<T>();
            Console.WriteLine("Database Pages received: " + databasePages.Count);

	        var lastUpdate = Preferences.LastPageUpdateTime<T>(Language, Location);
	        if (!forceRefresh && databasePages.Count != 0 && lastUpdate.AddHours(NoReloadTimeout) >= DateTime.Now)
	        {
	            return databasePages;
	        }
	        // if database is empty, do a full scan and not only from the latest update

	        var networkPages = CrossConnectivity.Current.IsConnected
	            ? await Policy
	                .Handle<WebException>()
	                .WaitAndRetryAsync
	                (
	                    retryCount: 5,
	                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
	                )
	                .ExecuteAsync(
	                    async () =>
	                        await LoadNetworkPages(new UpdateTime(databasePages.Count == 0 ? 0 : lastUpdate.Ticks)))
	            : null;

	        if (networkPages != null)
	        {
	            Console.WriteLine("Network Pages received: " + networkPages.Count);
	            var pagesIdMappingDictionary = new Dictionary<int, int>();
	            foreach (var page in databasePages.Where(page => !pagesIdMappingDictionary.ContainsKey(page.Id)))
	            {
	                pagesIdMappingDictionary.Add(page.Id, page.PrimaryKey);
	            }

	            //set language id so that we replace the language and dont add duplicates into the database!
	            foreach (var page in networkPages)
	            {
	                int networkPagePrimaryKey;
	                if (pagesIdMappingDictionary.TryGetValue(page.Id, out networkPagePrimaryKey))
	                {
	                    page.PrimaryKey = networkPagePrimaryKey;
	                }
	                page.LanguageId = Language.PrimaryKey;
                    page.AvailableLanguages?.ForEach(x => x.OwnPageId = page.Id);
	            }
	            await _persistenceService.InsertAll(networkPages);
	            Preferences.SetLastPageUpdateTime<T>(Language, Location);
	        }
	        return await _persistenceService.GetPages<T>(Language).DefaultIfFaulted(new List<T>());
	    }
	}
}
