using System;
using System.Collections.Generic;
using System.Linq;

namespace Integreat.Shared.ViewModels
{
    public class TabViewModel : BaseViewModel
    {
        public PagesViewModel PagesViewModel { get; }
        public EventPagesViewModel EventPagesViewModel { get; }

        public TabViewModel(PagesViewModel pagesViewModel, EventPagesViewModel eventPagesViewModel)
        {
            Title = "Tabs";
            Console.WriteLine("TabViewModel initialized");
            PagesViewModel = pagesViewModel;
            EventPagesViewModel = eventPagesViewModel;
        }

        public IEnumerable<PageViewModel> GetPages()
        {
            return PagesViewModel.LoadedPages.Union(EventPagesViewModel.EventPages);
        }
    }
}
