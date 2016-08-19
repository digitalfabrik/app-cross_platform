using System.Collections.Generic;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class DetailedPagesViewModel : BaseViewModel
    {
        private IEnumerable<PageViewModel> _pages;
        public IEnumerable<PageViewModel> Pages
        {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }
        
        public string Content { get; set; }


        private Command _itemTappedCommand;

        public Command ItemTappedCommand
        {
            get { return _itemTappedCommand; }
            set { SetProperty(ref _itemTappedCommand, value); }
        }

        private void OnTap(object sender)
        {
            var elem = sender as PageViewModel;
            elem?.ShowPageCommand.Execute(null);
        }

        public object LastTappedItem { get; set; }

        public DetailedPagesViewModel(IAnalyticsService analytics, PageViewModel parentPage, IEnumerable<PageViewModel> pages)
            : base(analytics)
        {
            Title = parentPage.Title;
            _pages = pages;
            Content = parentPage.Page.Content;
            _itemTappedCommand = new Command(OnTap);
        }
    }
}
