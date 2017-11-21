using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Factories;
using Integreat.Shared.Factories.Loader.Targets;
using Integreat.Shared.Factories.Services;
using Integreat.Shared.Models;

namespace Integreat.Shared.Utilities
{
    public class ShortnameParser
    {
        private readonly IDataLoadService _dataLoadService;

        public ShortnameParser()
        {
            this._dataLoadService = DataLoadServiceFactory.Create();
        }


        public async Task<Location> getLocation(string shortname)
        {
            if (this._dataLoadService == null)
                return null;

            //create dataloader
            LocationsDataLoader Locationdataloader = new LocationsDataLoader(this._dataLoadService);

            List<Location> locations = new List<Location>(await Locationdataloader.Load(false, null));

            Location location = locations.Where(l => l.Path == "/" + shortname + "/").First();

            return location;
        }

        public async Task<Language> getLanguage(string shortname, Location location)
        {
            if (this._dataLoadService == null||location==null)
                return null;

            //create dataloader
            LanguagesDataLoader Languagedataloader = new LanguagesDataLoader(this._dataLoadService);
            List<Language> languages = new List<Language>(await Languagedataloader.Load(false, location, null));

            Language language = languages.Where(l => l.ShortName == shortname).First();
            return language;
        }
    }
}
