using System;
using System.Linq;
using Integreat.Shared.Models;
using Integreat.Shared.Services.Loader;
using Integreat.Shared.Services.Tracking;

namespace Integreat.Shared.ViewModels
{
	public class DisclaimerViewModel : BaseViewModel
	{
	    private string _content;

	    public string Content
	    {
	        get { return _content; }
            set { SetProperty(ref _content, value); }
	    }
        private readonly DisclaimerLoader _loader;

        public DisclaimerViewModel(IAnalyticsService analytics, Language language, Location location, Func<Language, Location, DisclaimerLoader> disclaimerLoaderFactory)
        :base(analytics) {
            Title = "Disclaimer";
            
            _loader = disclaimerLoaderFactory(language, location);
            Refresh();
        }

        private async void Refresh(bool forceRefresh = false)
        {
            try
            {
                IsBusy = true;
                var pages = await _loader.Load(forceRefresh);
                Content = string.Join("<br><br>", pages.Select(x => x.Content));
                Console.WriteLine("Disclaimer content: " + Content);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
