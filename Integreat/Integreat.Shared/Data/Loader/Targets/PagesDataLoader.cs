using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets {
    public class PagesDataLoader : IDataLoader {

        public const string FileNameConst = "pagesV2";
        public string FileName => FileNameConst;
        public DateTime LastUpdated {
            get { return Preferences.LastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation); }
            set { Preferences.SetLastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation, DateTime.Now); }
        }

        public string Id => "Id";

        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;


        public PagesDataLoader(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }


        public Task<Collection<Page>> Load(bool forceRefresh, Language forLanguage, Location forLocation) {
            _lastLoadedLocation = forLocation;
            _lastLoadedLanguage = forLanguage;

            // action which will be executed on newly loaded data
            Action<Collection<Page>> worker = pages => {
                foreach (var page in pages) {
                    page.PrimaryKey = Page.GenerateKey(page.Id, forLocation, forLanguage);
                    if (!"".Equals(page.ParentJsonId) && page.ParentJsonId != null) {
                        page.ParentId = Page.GenerateKey(page.ParentJsonId, forLocation, forLanguage);
                    }
                    
                }
            };

            // action which will be executed on the merged list of loaded and cached data
            Action<Collection<Page>> persistWorker = pages =>
            {
                // remove all pages which status is "trash"
                var itemsToRemove = pages.Where(x => x.Status == "trash").ToList();
                foreach (var page in itemsToRemove)
                {
                    pages.Remove(page);
                }
            };

            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, () => _dataLoadService.GetPages(forLanguage, forLocation, new UpdateTime(LastUpdated.Ticks)), worker, persistWorker);
        }
    }
}
