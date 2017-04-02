using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets {
    public class LocationsDataLoader : IDataLoader<Collection<Location>> {
        public string FileName => "locations";
        public DateTime LastUpdated
        {
            get { return Preferences.LastLocationUpdateTime(); }
            set { Preferences.SetLastLocationUpdateTime(value); }
        }

        public string Id => null;

        private IDataLoadService _dataLoadService;

        public LocationsDataLoader(IDataLoadService dataLoadService) {
            _dataLoadService = dataLoadService;
        }


        public Task<Collection<Location>> Load(bool forceRefresh)
        {
            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, _dataLoadService.GetLocations);
        }

    }
}
