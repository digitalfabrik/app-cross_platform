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

        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }


        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator)
        : base (analytics) {
            Title = "Select Language";
            ToolbarItems = new List<ToolbarItem>();
            var converter = new FileImageSourceConverter();
            ToolbarItems.Add(new ToolbarItem() {Text = "asd"});
        }
    }
}
