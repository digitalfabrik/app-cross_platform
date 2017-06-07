using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets {
    public class LanguagesDataLoader : IDataLoader {
        public const string FileNameConst = "languagesV1";
        public string FileName => FileNameConst;
        public DateTime LastUpdated {
            get { return Preferences.LastLanguageUpdateTime(_lastLoadedLocation); }
            set { Preferences.SetLastLanguageUpdateTime(_lastLoadedLocation, value); }
        }

        public string Id => null;

        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;

        public LanguagesDataLoader(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }


        /// <summary>
        /// Loads the languages for the given location.
        /// </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="forLocation">The location to load the languages for.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the languages.</returns>
        public async Task<Collection<Language>> Load(bool forceRefresh, Location forLocation, Action<string> errorLogAction = null) {
            _lastLoadedLocation = forLocation;

            Action<Collection<Language>> worker = x =>
            {
                // set the location properties for each loaded language
                foreach (var language in x)
                {
                    language.Location = forLocation;
                    language.PrimaryKey = forLocation.Id + "_" + language.Id;
                }
            };

            var languages = await DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, () => _dataLoadService.GetLanguages(forLocation), errorLogAction, worker);
            

            return languages;
        }
    }
}
