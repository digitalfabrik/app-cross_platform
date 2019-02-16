using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Data.Services;
using Integreat.Model;
using Integreat.Utilities;

namespace Integreat.Data.Loader.Targets
{
    /// <inheritdoc />
    public class LanguagesDataLoader : IDataLoader
    {
        private const string FileNameConst = "languagesV3";
        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;

        public LanguagesDataLoader(IDataLoadService dataLoadService)
        {
            _dataLoadService = dataLoadService;
        }

        public DateTime LastUpdated
        {
            get => Preferences.LastLanguageUpdateTime(_lastLoadedLocation);
            set => Preferences.SetLastLanguageUpdateTime(_lastLoadedLocation, value);
        }

        public string FileName { get; private set; }

        public string Id => null;

        /// <summary> Loads the languages for the given location. </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="forLocation">The location to load the languages for.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the languages.</returns>
        public async Task<Collection<Language>> Load(bool forceRefresh, Location forLocation,
            Action<string> errorLogAction = null)
        {
            _lastLoadedLocation = forLocation;

            FileName = $"{_lastLoadedLocation.NameWithoutStreetPrefix}_{FileNameConst}.json";


            Action<Collection<Language>> worker = x =>
            {
                // set the location properties for each loaded language
                foreach (var language in x)
                {
                    language.Location = forLocation;
                    language.PrimaryKey = forLocation.Id + "_" + language.Id;
                }
            };

            return await DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this,
                () => _dataLoadService.GetLanguages(forLocation), errorLogAction, worker);
        }
    }
}