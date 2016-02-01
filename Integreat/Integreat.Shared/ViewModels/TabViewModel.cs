using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Models;

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

        // this will refresh page and events automatically
        public void SetLanguage(Language language)
        {
            PagesViewModel.Language = language;
            EventPagesViewModel.Language = language;
        }

        public void SetLocation(Location location)
        {
            PagesViewModel.Location = location;
            EventPagesViewModel.Location = location;
        }
    }
}
