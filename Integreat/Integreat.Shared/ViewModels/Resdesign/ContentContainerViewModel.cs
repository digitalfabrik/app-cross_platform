using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Integreat.Shared.ApplicationObjects;
using Integreat.Shared.Models;
using Integreat.Shared.Pages;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace Integreat.Shared.ViewModels.Resdesign
{
    public class ContentContainerViewModel : BaseViewModel
    {
        private INavigator _navigator;

        private List<ToolbarItem> _toolbarItems;
        private Func<LocationsViewModel> _locationFactory;
        private Func<EventPageViewModel> _eventPageFactory;
        private Func<MainPageViewModel> _mainPageFactory;
        private IViewFactory _viewFactory;

        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator, Func<LocationsViewModel> locationFactory, Func<MainPageViewModel> mainPageFactory, Func<EventPageViewModel> eventPageFactory, IViewFactory viewFactory)
        : base (analytics) {
            Title = "Select Language";
            _navigator = navigator;
            _navigator.HideToolbar(this);
            _locationFactory = locationFactory;
            _mainPageFactory = mainPageFactory;
            _eventPageFactory = eventPageFactory;

            _viewFactory = viewFactory;

            ToolbarItems = new List<ToolbarItem>();
            var converter = new FileImageSourceConverter();
            ToolbarItems.Add(new ToolbarItem() {Text = "asd"});
        }

        /// <summary>
        /// Opens the location selection as modal page and pops them both when the language was selected.
        /// </summary>
        public async void OpenLocationSelection()
        {
            var vm = _locationFactory();
            vm.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
             await _navigator.PushModalAsync(vm);
        }

        /// <summary>
        /// Called when [language selected].
        /// </summary>
        /// <param name="languageViewModel">The languageViewModel.</param>
        private async void OnLanguageSelected(object languageViewModel)
        {
            await _navigator.PopModalAsync();
        }

        public async void CreateMainView(IList<Page> children, IList<ToolbarItem> toolbarItems)
        {
            ToolbarItems.Add(new ToolbarItem() { Text = "testse", Icon = "globe.png" });

            children.Add(new NavigationPage(_viewFactory.Resolve<MainContentPageViewModel>()) { Title = "Main" });
            children.Add(new NavigationPage(_viewFactory.Resolve<ExtrasContentPageViewModel>()) { Title = "Extras" });
            children.Add(new NavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()) { Title = "Events" });
            children.Add(new NavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()) { Title = "Settings" });
        }
    }
}
