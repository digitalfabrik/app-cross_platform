using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Integreat
{
    public class Network
    {
        public Network()
        {
        }

        public List<Integreat.Location> getAvailableLocations() {
            using (var WC = new System.Net.WebClient())
            {
                string json = WC.DownloadString(Konstanten.BaseUri + Konstanten.getAvailableLocationsUri);
                var result =  JsonConvert.DeserializeObject<List<Location>>(json);
                return result;             
            }
        } 

        public List<Integreat.Language> getAvailableLanguages(Location location) {
            using (var WC = new System.Net.WebClient())
            {
                string json = WC.DownloadString(Konstanten.BaseUri +  string.Format(Konstanten.getAvailableLanguagesUri, location.path));
                var result =  JsonConvert.DeserializeObject<List<Integreat.Language>>(json);
                return result;             
            }
        }

        public List<Page> getPages(Integreat.Location location, Integreat.Language language) {
            using (var WC = new System.Net.WebClient())
            {
                string json = WC.DownloadString(Konstanten.BaseUri +  string.Format(Konstanten.getPagesUri, location.path, language.code)+"?since=2014-12-31T16%3A00%3A00-0800");
                var result =  JsonConvert.DeserializeObject<List<Page>>(json);
                return result;             
            }

        }

        public string loadCityImage(Location location) {
            if (location.name == "muenchen")
            {
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/1/14/M%C3%BCnchen_Panorama.JPG/300px-M%C3%BCnchen_Panorama.JPG";
            }
            else if (location.name == "augsburg")
            {
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d3/Augsburg_-_Markt.jpg/297px-Augsburg_-_Markt.jpg";
            }
            else if (location.name == "pre arrival")
            {
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/2/26/EU-Germany.svg/800px-EU-Germany.svg.png";
            }
            else if (location.name == "deutschland")
            {
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Brandenburger_Tor_abends.jpg/300px-Brandenburger_Tor_abends.jpg";
            }
            else
            {
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/1/14/M%C3%BCnchen_Panorama.JPG/300px-M%C3%BCnchen_Panorama.JPG";
            }
        }
    }
}

