using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.ViewModels;

namespace Integreat.Shared.Utilities
{
    public class UrlParser
    {
        private List<string> _segmentList = new List<string>();
        private Uri _url;

        public UrlParser()
        {

        }

        public UrlParser(Uri url){
            Url = url;
        }

        #region properies
        public List<string> SegmentList
        {
            get { return this._segmentList; }
            set { this._segmentList = value; }
        }

        public Uri Url
        {
            get { return this._url; }
            set 
            {
                if(Url !=value){
                    this._url = value;
                    generateSegments();
                }
            }
        }

        #endregion

        #region functions
        private void generateSegments(){
            SegmentList = (System.Collections.Generic.List<string>)Url.Segments.Where(s => s != "/").Select(s => s.Trim(new Char[] { '/' }));
        }

        private LanguagesViewModel getLanguagesViewModel(Location location){
            return null;
        }

        public BaseViewModel GetPageViewModel()
        {
            if (SegmentList.IsNullOrEmpty())
                return null;

            //example:
            //regensburg/de/page

            String locationShortname = !SegmentList[0].IsNullOrEmpty() ? SegmentList[0] : String.Empty;
            String languageShortname = !SegmentList[1].IsNullOrEmpty() ? SegmentList[1] : String.Empty;

            if (locationShortname == null)
                return null; //just show standard page

            ShortnameParser shortnameparser = new ShortnameParser();
            //get location
            Location location = Task.Run(() => shortnameparser.getLocation(locationShortname)).Result;
            if (location == null)
                return null;
            
            //get language
            Language language = null;
            if (!languageShortname.IsNullOrEmpty())
            {
                language = Task.Run(() => shortnameparser.getLanguage(languageShortname, location)).Result;
            }

            if (languageShortname.IsNullOrEmpty() || language == null)
                return getLanguagesViewModel(location);

            return null;
        }
        #endregion
    }
}
