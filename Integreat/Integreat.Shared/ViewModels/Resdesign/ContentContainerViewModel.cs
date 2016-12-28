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

        private LocationsViewModel _locationsViewModel; // view model for when OpenLocationSelection is called
        private IList<Page> _children; // children pages of this ContentContainer

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
            _navigator.HideToolbar(this);
        }

        /// <summary>
        /// Opens the location selection as modal page and pops them both when the language was selected.
        /// </summary>
        public async void OpenLocationSelection()
        {
            _locationsViewModel = _locationFactory();
            _locationsViewModel.OnLanguageSelectedCommand = new Command<object>(OnLanguageSelected);
             await _navigator.PushModalAsync(_locationsViewModel);
        }

        /// <summary>
        /// Called when [language selected].
        /// </summary>
        /// <param name="languageViewModel">The languageViewModel.</param>
        private async void OnLanguageSelected(object languageViewModel)
        {
            await _navigator.PopModalAsync();

            // refresh every page (this is for the case, that we changed the language, while the main view is already displayed. Therefore we need to update the pages, since the location or language has most likely changed)
            RefreshAll();
        }

        public async void CreateMainView(IList<Page> children, IList<ToolbarItem> toolbarItems)
        {
            _children = children;
            var navigationPage = new NavigationPage(_viewFactory.Resolve<MainContentPageViewModel>()) { Title = "Main", BarTextColor = (Color)Application.Current.Resources["textColor"] };
           // navigationPage.ToolbarItems.Add(new ToolbarItem() { Text = "Language", Icon = "globe.png" });
            children.Add(navigationPage);
            
            navigationPage = new NavigationPage(_viewFactory.Resolve<ExtrasContentPageViewModel>()) { Title = "Extras", BarTextColor = (Color)Application.Current.Resources["textColor"] };
            children.Add(navigationPage);


            children.Add(new NavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()) { Title = "Events", BarTextColor = (Color)Application.Current.Resources["textColor"] });
            children.Add(new NavigationPage(_viewFactory.Resolve<EventsContentPageViewModel>()) { Title = "Settings", BarTextColor = (Color)Application.Current.Resources["textColor"] });


            // refresh every page
            RefreshAll();
        }

        private void RefreshAll()
        {
            foreach (var child in _children) {
                var navPage = child as NavigationPage;
                var page = navPage?.CurrentPage as BaseContentPage;
                if (page == null) continue;
                page.Title = _locationsViewModel?.SelectedLocation?.Name;
                page.Refresh();
            }
        }
    }
}
