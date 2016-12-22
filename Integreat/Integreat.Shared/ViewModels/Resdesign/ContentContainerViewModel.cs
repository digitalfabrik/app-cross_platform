using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.Resdesign
{
    public class ContentContainerViewModel : BaseViewModel
    {
        private INavigator _navigator;

        private List<ToolbarItem> _toolbarItems;
        private Func<LocationsViewModel> _locationFactory;

        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator, Func<LocationsViewModel> locationFactory)
        : base (analytics) {
            Title = "Select Language";
            _navigator = navigator;
            _locationFactory = locationFactory;
            ToolbarItems = new List<ToolbarItem>();
            var converter = new FileImageSourceConverter();
            ToolbarItems.Add(new ToolbarItem() {Text = "asd"});
        }

        public async void OpenLocationSelection()
        {
            var vm = _locationFactory();
            await _navigator.PushModalAsync(vm);
        }
    }
}
