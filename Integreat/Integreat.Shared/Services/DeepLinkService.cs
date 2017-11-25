using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Factories.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.ViewModels;

namespace Integreat.Shared.Utilities
{
    public class DeepLinkService:IDeepLinkService
    {
        private readonly INavigator _navigator;


        private List<string> _segmentList = new List<string>();
        private Uri _url;
        private String _locationShortname;
        private String _languageShortname;

        public DeepLinkService(INavigator navigator)
        {
            this._navigator = navigator;
        }


        #region properies
        public List<string> SegmentList
        {
            get { return this._segmentList; }
            private set { this._segmentList = value; }
        }

        public Uri Url
        {
            get { return this._url; }
            set 
            {
                if(Url !=value){
                    this._url = value;
                    generateSegments();
                    generateShortnames();
                }
            }
        }

        #endregion

        #region functions
        private void generateSegments(){
            SegmentList = Url.Segments.Where(s => s != "/").Select(s => s.Trim(new Char[] { '/' })).ToList();
        }

        private void generateShortnames(){
            this._locationShortname = !SegmentList[0].IsNullOrEmpty() ? SegmentList[0] : String.Empty;
            this._languageShortname = !SegmentList[1].IsNullOrEmpty() ? SegmentList[1] : String.Empty;
        }

        public void Navigate()
        {
            if (SegmentList.IsNullOrEmpty()||this._locationShortname.IsNullOrEmpty())
                return;

            //example:
            //regensburg/de/page


            ShortnameParser shortnameparser = new ShortnameParser();
            //get location
            Location location = Task.Run(() => shortnameparser.getLocation(this._locationShortname)).Result;
            if (location == null)
                return;

            //set location in preferences
            Preferences.SetLocation(location);

            //get language
            Language language = null;
            if (!this._languageShortname.IsNullOrEmpty())
            {
                language = Task.Run(() => shortnameparser.getLanguage(this._languageShortname, location)).Result;
            }

            if (language == null)
                return;

            //set language in preference
            Preferences.SetLanguage(location, language);
        }
        #endregion
    }
}
