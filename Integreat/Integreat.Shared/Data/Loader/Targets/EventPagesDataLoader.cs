using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Data.Loader.Targets
{
    /// <inheritdoc />
    public class EventPagesDataLoader : IDataLoader
    {
        private const string _FileNameConst = "eventsV3";
        private string _FileName;

        public DateTime LastUpdated
        {
            get => Preferences.LastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation);
            // ReSharper disable once ValueParameterNotUsed
            set => Preferences.SetLastPageUpdateTime<EventPage>(_lastLoadedLanguage, _lastLoadedLocation, DateTime.Now);
        }

        public string FileName
        {
            //get just for fallback stuff
            get => _FileName;
            private set
            {
                _FileName = value;
            }
        }

        /// <inheritdoc />
        public string Id => "Id";

        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;

        public EventPagesDataLoader(IDataLoadService dataLoadService)
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

            FileName = _lastLoadedLocation.NameWithoutStreetPrefix + "_" + _lastLoadedLanguage.ShortName + "_" + _FileNameConst;

            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this,
                () => _dataLoadService.GetEventPages(forLanguage, forLocation),
                errorLogAction, null, null);
        }
    }
}