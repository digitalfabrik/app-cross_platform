using System;
using System.Collections.ObjectModel;
using System.Linq;
using Integreat.Shared.Services.Loader;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
	public class DisclaimerViewModel : BaseViewModel
    {
        public ObservableCollection<PageViewModel> Pages {get; set;}
	    private readonly DisclaimerLoader _loader;
	    private readonly Func<Models.Page, PageViewModel> _pageFactory;

        public DisclaimerViewModel(Func<Models.Page, PageViewModel> pageFactory, Func<DisclaimerLoader> disclaimerLoaderFactory)
        {
            Title = "Information";
            Pages = new ObservableCollection<PageViewModel>();

            _pageFactory = pageFactory;
            _loader = disclaimerLoaderFactory();
            Refresh();
        }
        private Command _loadPagesCommand;
        public Command LoadPagesCommand => _loadPagesCommand ??  (_loadPagesCommand = new Command(Refresh));

        private async void Refresh()
        {
            var pages = await _loader.Load();
            Pages.Clear();
            Pages.AddRange(pages.Select(x => _pageFactory(x)));
        }
    }
}
