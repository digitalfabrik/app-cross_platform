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
                }
            }
        }

        #endregion

        #region functions
        private void generateSegments(){
            SegmentList = (System.Collections.Generic.List<string>)Url.Segments.Where(s => s != "/").Select(s => s.Trim(new Char[] { '/' }));
        }


        public void Navigate()
        {
            if (SegmentList.IsNullOrEmpty())
                return;

            //example:
            //regensburg/de/page

            String locationShortname = !SegmentList[0].IsNullOrEmpty() ? SegmentList[0] : String.Empty;
            String languageShortname = !SegmentList[1].IsNullOrEmpty() ? SegmentList[1] : String.Empty;

            if (locationShortname == null)
                return; //just show standard page

            ShortnameParser shortnameparser = new ShortnameParser();
            //get location
            Location location = Task.Run(() => shortnameparser.getLocation(locationShortname)).Result;
            if (location == null)
                return;

            //get language
            Language language = null;
            if (!languageShortname.IsNullOrEmpty())
            {
                language = Task.Run(() => shortnameparser.getLanguage(languageShortname, location)).Result;
            }

            if (languageShortname.IsNullOrEmpty() || language == null)
                //location viewmodel
                return;

            return;
        }
        #endregion
    }
}
