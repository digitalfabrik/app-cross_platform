using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.ViewModels;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// Service for DeepLinks
    /// </summary>
    public class DeepLinkService : IDeepLinkService
    {
        private readonly INavigator _navigator;
        private Uri _url;
        private string _locationShortname;
        private string _languageShortname;

        #region properies
        /// <summary> Gets the segment list. </summary>
        /// <value> The segment list. </value>
        public ICollection<string> SegmentList { get; private set; } = new List<string>();

        public DeepLinkService(INavigator navigator)
        {
            _navigator = navigator;
        }

        public Uri Url
        {
            private get => _url;
            set
            {
                if (Url == value) return;
                _url = value;
                GenerateSegments();
                GenerateShortnames();
            }
        }

        #endregion

        #region functions
        private void GenerateSegments()
        {
            SegmentList = Url.Segments.Where(s => s != "/").Select(s => s.Trim('/')).ToList();
        }

        private void GenerateShortnames()
        {
            _locationShortname = !SegmentList.ElementAtOrDefault(0).IsNullOrEmpty() ? SegmentList.ElementAt(0) : string.Empty;
            _languageShortname = !SegmentList.ElementAtOrDefault(1).IsNullOrEmpty() ? SegmentList.ElementAt(1) : string.Empty;
        }

        public void Navigate(IShortnameParser parser)
        {
            if (SegmentList.IsNullOrEmpty() || _locationShortname.IsNullOrEmpty())
                return;

            //example:
            //regensburg/de/page
            //get location
            var location = Task.Run(() => parser.GetLocation(_locationShortname)).Result;
            if (location == null)
                return;

            //set location in preferences
            Preferences.SetLocation(location);

            //get language
            Language language = null;
            if (!_languageShortname.IsNullOrEmpty())
            {
                language = Task.Run(() => parser.GetLanguage(_languageShortname, location)).Result;
            }

            if (language == null) return;

            //set language in preference
            Preferences.SetLanguage(location, language);

            MainContentPageViewModel.Current.ContentContainer.RefreshAll();

            if(!SegmentList.ElementAtOrDefault(2).IsNullOrEmpty()){
                //string to page
                var pageViewModel = MainContentPageViewModel.Current.LoadedPages.Where(pagevm => pagevm.Title == SegmentList.ElementAt(2));

                //simulate pageTabbed
                MainContentPageViewModel.Current.OnPageTapped(pageViewModel);
            }
        }
        #endregion
    }
}
