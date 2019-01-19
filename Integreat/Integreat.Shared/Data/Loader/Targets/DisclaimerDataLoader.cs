﻿using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Integreat.Shared.Data.Loader.Targets
{
    /// <inheritdoc />
    public class DisclaimerDataLoader : IDataLoader
    {
        private const string FileNameConst = "disclaimerV3";

        public DateTime LastUpdated
        {
            get => Preferences.LastPageUpdateTime<Disclaimer>(_lastLoadedLanguage, _lastLoadedLocation);
            // ReSharper disable once ValueParameterNotUsed
            set => Preferences.SetLastPageUpdateTime<Disclaimer>(_lastLoadedLanguage, _lastLoadedLocation, DateTime.Now);
        }

        //get just for fallback stuff
        public string FileName { get; private set; }


        public string Id => "Id";

        private readonly IDataLoadService _dataLoadService;
        private Location _lastLoadedLocation;
        private Language _lastLoadedLanguage;

        public DisclaimerDataLoader(IDataLoadService dataLoadService)
        {
            _dataLoadService = dataLoadService;
        }

        /// <summary> Loads the disclaimer. </summary>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <param name="forLanguage">Which language to load for.</param>
        /// <param name="forLocation">Which location to load for.</param>
        /// <param name="errorLogAction">The error log action.</param>
        /// <returns>Task to load the disclaimer.</returns>
        public Task<Collection<Disclaimer>> Load(bool forceRefresh, Language forLanguage, Location forLocation, Action<string> errorLogAction = null)
        {
            _lastLoadedLocation = forLocation;
            _lastLoadedLanguage = forLanguage;

            FileName = $"{_lastLoadedLocation.NameWithoutStreetPrefix}_{_lastLoadedLanguage.ShortName}_{FileNameConst}.json";

            return DataLoaderProvider.ExecuteLoadMethod(forceRefresh, this, () => Helper(), errorLogAction);
        }

        private Task<Collection<Disclaimer>> Helper()
        {
            var c = new Collection<Disclaimer>();
            return Task.Run(() =>
            {
                var d = _dataLoadService.GetDisclaimer(_lastLoadedLanguage, _lastLoadedLocation).Result;

                if (d != null)
                {
                    c.Add(d);
                }

                return c;
            });
        }
    }
}