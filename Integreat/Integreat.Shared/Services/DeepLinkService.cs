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
    /// <summary>
    /// Service for DeepLinks
    /// </summary>
    public class DeepLinkService : IDeepLinkService
    {
        private readonly DataLoaderProvider _dataLoaderProvider;
        private readonly Func<Page, PageViewModel> _pageViewModelFactory;
        private readonly IShortnameParser _shortnameParser;
        private Uri _url;
        private string _locationShortname;
        private string _languageShortname;

        #region properies
        /// <summary> Gets the segment list. </summary>
        /// <value> The segment list. </value>
        public ICollection<string> SegmentList { get; private set; } = new List<string>();

        public DeepLinkService(DataLoaderProvider dataLoaderProvider, Func<Page, PageViewModel> pageViewModelFactory, IShortnameParser shortnameParser)
        {
            _dataLoaderProvider = dataLoaderProvider;
            _pageViewModelFactory = pageViewModelFactory;
            _shortnameParser = shortnameParser;
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
            _locationShortname = SegmentList.ElementAtOrDefault(0);
            _languageShortname = SegmentList.ElementAtOrDefault(1);
        }

        public async Task Navigate()
        {
            if (SegmentList.IsNullOrEmpty() || _locationShortname.IsNullOrEmpty())
                return;

            //example:
            //regensburg/de/page
            //get location
            var location = Task.Run(() => _shortnameParser.GetLocation(_locationShortname)).Result;
            if (location == null)
                return;

            //set location in preferences
            Preferences.SetLocation(location);

            //get language
            Language language = null;
            if (!_languageShortname.IsNullOrEmpty())
            {
                language = Task.Run(() => _shortnameParser.GetLanguage(_languageShortname, location)).Result;
            }

            if (language == null) return;

            //set language in preference
            Preferences.SetLanguage(location, language);

           // MainContentPageViewModel.Current.ContentContainer.RefreshAll(true);
           // MainContentPageViewModel.Current.MetaDataChangedCommand.Execute(null);

            if(!SegmentList.ElementAtOrDefault(2).IsNullOrEmpty()){
                //string to page
                var lastSegment = SegmentList.Last();

                var pageCollection = await _dataLoaderProvider.PagesDataLoader.Load(false, language, location);
                try{
                    var page = pageCollection.First(p => p.Permalinks.UrlPage.Split('/').Last() == lastSegment);
                    if (page != null)
                    {
                        var pagevm = _pageViewModelFactory(page);
                        SetChildProperty(ref pagevm, pageCollection);

                        //unfortunately MainTwoLevelPage doesn't work
                        //so this is a quick fix
                        if(pagevm.Children.Any())
                            return;
                        

                        //simulate pageTabbed
                        MainContentPageViewModel.Current.OnPageTapped(pagevm);
                    }
                }catch(Exception e){
                    Console.WriteLine(e.Message);
                }

            }
        }

        private void SetChildProperty(ref PageViewModel pageViewModel, IList<Page> pageCollection)
        {
            //add pages to children
            var primaryKey = pageViewModel.Page.PrimaryKey;
            var children = pageCollection.Where(page => page.ParentId == primaryKey).ToList();
            pageViewModel.Children = children.Select(page => _pageViewModelFactory(page)).ToList();
        }
        #endregion
    }
}
