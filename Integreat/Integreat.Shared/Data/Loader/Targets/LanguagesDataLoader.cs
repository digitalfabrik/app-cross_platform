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


        public async Task<Collection<Language>> Load(bool forceRefresh, Location forLocation) {
            _lastLoadedLocation = forLocation;

            Action<Collection<Language>> worker = x =>
            {
                // set the location properties for each loaded language
                foreach (var language in x)
                {
                    language.Location = forLocation;
                    language.LocationId = forLocation.Id;
                    language.PrimaryKey = forLocation.Id + "_" + language.Id;
                }
            };

            var languages = await DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, () => _dataLoadService.GetLanguages(forLocation), worker);
            

            return languages;
        }
    }
}
