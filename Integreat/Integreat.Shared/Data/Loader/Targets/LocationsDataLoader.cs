using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets {
    public class LocationsDataLoader : IDataLoader {

        public const string FileNameConst = "locationsV1";
        public string FileName => FileNameConst;
        public DateTime LastUpdated
        {
            get { return Preferences.LastLocationUpdateTime(); }
            set { Preferences.SetLastLocationUpdateTime(value); }
        }

        public string Id => null;

        private readonly IDataLoadService _dataLoadService;

        public LocationsDataLoader(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }


        /// <summary>
        /// Loads the locations.
        /// </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the collection of locations.</returns>
        public Task<Collection<Location>> Load(bool forceRefresh, Action<string> errorLogAction = null)
        {
            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, _dataLoadService.GetLocations, errorLogAction);
        }

    }
}
