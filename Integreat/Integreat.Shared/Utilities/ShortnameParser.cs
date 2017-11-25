using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Factories;
using Integreat.Shared.Factories.Loader.Targets;
using Integreat.Shared.Models;

namespace Integreat.Shared.Utilities
{
    /// <inheritdoc />
    public class ShortnameParser : IShortnameParser
    {
        private readonly IDataLoadService _dataLoadService;

        public ShortnameParser(IDataLoadService dataLoadService)
        {
            _dataLoadService = dataLoadService;
        }

        /// <inheritdoc />
        public async Task<Location> GetLocation(string shortname)
        {
            if (_dataLoadService == null) return null;

            //create dataloader
            var locationdataloader = new LocationsDataLoader(_dataLoadService);

            var locations = new List<Location>(await locationdataloader.Load(false));

            var location = locations.First(l => l.Path == "/" + shortname + "/");

            return location;
        }

        /// <inheritdoc />
        public async Task<Language> GetLanguage(string shortname, Location location)
        {
            if (_dataLoadService == null || location == null) return null;

            //create dataloader
            var languagedataloader = new LanguagesDataLoader(_dataLoadService);
            var languages = new List<Language>(await languagedataloader.Load(false, location));

            var language = languages.First(l => l.ShortName == shortname);
            return language;
        }
    }

    /// <summary>
    /// The ShortNameParser parse a string to a location and language instance
    /// </summary>
    public interface IShortnameParser
    {
        /// <summary>  Gets the location from a shortname string. </summary>
        /// <param name="shortname">The shortname.</param>
        /// <returns>Location</returns>
        Task<Location> GetLocation(string shortname);
        /// <summary> Gets the language. </summary>
        /// <param name="shortname">The shortname.</param>
        /// <param name="location">The location.</param>
        /// <returns>Language</returns>
        Task<Language> GetLanguage(string shortname, Location location);
    }
}
