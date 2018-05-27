using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Data.Loader.Targets
{
    public class PushNotificationsDataLoader : IDataLoader
    {
        public const string FileNameConst = "pushnotificationsV1";

        /// <inheritdoc />
        public string FileName => FileNameConst;
        
        public DateTime LastUpdated
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string Id => "Id";
        
        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;

        public PushNotificationsDataLoader(IDataLoadService dataLoadService)
        {
            _dataLoadService = dataLoadService;
        }

		/// <summary> Loads the event pages. </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="forLanguage">Which language to load for.</param>
        /// <param name="forLocation">Which location to load for.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the event pages.</returns>
        public Task<Collection<EventPage>> Load(bool forceRefresh, Language forLanguage, Location forLocation,
            Action<string> errorLogAction = null)
        {
            _lastLoadedLocation = forLocation;
            _lastLoadedLanguage = forLanguage;

            Action<Collection<EventPage>> worker = pages =>
            {
                foreach (var page in pages)
                {
                    page.PrimaryKey = Page.GenerateKey(page.Id, forLocation, forLanguage);
                    if (!"".Equals(page.ParentJsonId) && page.ParentJsonId != null)
                    {
                        page.ParentId = Page.GenerateKey(page.ParentJsonId, forLocation, forLanguage);
                    }
                }
            };

            // action which will be executed on the merged list of loaded and cached data
            Action<Collection<EventPage>> persistWorker = pages =>
            {

            };

            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this,
                () => _dataLoadService.GetEventPages(forLanguage, forLocation, new UpdateTime(LastUpdated.Ticks)),
                errorLogAction, worker, persistWorker);
        }
        /// <summary>
        /// Add the specified eventPage.
        /// </summary>
		public void Add(EventPage eventPage)
		{
			if(eventPage !=null)
			{
				Task.Run(() => DataLoaderProvider.AddObject(eventPage, this));
			}
		}
    }
}