using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets
{
    /// <inheritdoc />
    public class LocationsDataLoader : IDataLoader
    {
        private const string FileNameConst = "locationsV3.json";
        private readonly IDataLoadService _dataLoadService;
        public string FileName => FileNameConst;
        public DateTime LastUpdated
        {
            get => Preferences.LastLocationUpdateTime();
            set => Preferences.SetLastLocationUpdateTime(value);
        }

        public string Id => null;



        public LocationsDataLoader(IDataLoadService dataLoadService) => _dataLoadService = dataLoadService;

        /// <summary> Loads the locations. </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the collection of locations.</returns>
        public Task<Collection<Location>> Load(bool forceRefresh, Action<string> errorLogAction = null)
            => DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, _dataLoadService.GetLocations, errorLogAction);
    }
}