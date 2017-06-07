using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets {
    public class DisclaimerDataLoader : IDataLoader {
        public const string FileNameConst = "disclaimerV1";
        public string FileName => FileNameConst;
        public DateTime LastUpdated {
            get { return Preferences.LastPageUpdateTime<Disclaimer>(_lastLoadedLanguage, _lastLoadedLocation); }
            set { Preferences.SetLastPageUpdateTime<Disclaimer>(_lastLoadedLanguage, _lastLoadedLocation, DateTime.Now); }
        }

        public string Id => "Id";
        
        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;

        public DisclaimerDataLoader(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }


        /// <summary>
        /// Loads the disclaimer.
        /// </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="forLanguage">Which language to load for.</param>
        /// <param name="forLocation">Which location to load for.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the disclaimer.</returns>
        public Task<Collection<Disclaimer>> Load(bool forceRefresh, Language forLanguage, Location forLocation, Action<string> errorLogAction = null)
        {
            _lastLoadedLocation = forLocation;
            _lastLoadedLanguage = forLanguage;

            Action<Collection<Disclaimer>> worker = pages => {
                foreach (var page in pages) {
                    page.PrimaryKey = Page.GenerateKey(page.Id, forLocation, forLanguage);
                    //page.LanguageId = forLanguage.PrimaryKey;
                    //page.Language = forLanguage;
                    if (!"".Equals(page.ParentJsonId) && page.ParentJsonId != null) {
                        page.ParentId = Page.GenerateKey(page.ParentJsonId, forLocation, forLanguage);
                    }
                }
            };

            // action which will be executed on the merged list of loaded and cached data
            Action<Collection<Disclaimer>> persistWorker = pages => {
                // remove all pages which status is "trash"
                var itemsToRemove = pages.Where(x => x.Status == "trash").ToList();
                foreach (var page in itemsToRemove) {
                    pages.Remove(page);
                }
            };

            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, () => _dataLoadService.GetDisclaimers(forLanguage, forLocation, new UpdateTime(LastUpdated.Ticks)), errorLogAction, worker, persistWorker);
        }
    }
}
