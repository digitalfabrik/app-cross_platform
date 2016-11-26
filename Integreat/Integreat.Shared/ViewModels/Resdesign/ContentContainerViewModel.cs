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
        private string _fuck;

        public List<ToolbarItem> ToolbarItems {
            get { return _toolbarItems; }
            set { SetProperty(ref _toolbarItems, value); }
        }

        public string Fuck
        {
            get { return _fuck; }
            set { SetProperty(ref _fuck, value); }
        }

        public object Ass { get; set; }

        public ContentContainerViewModel(IAnalyticsService analytics, INavigator navigator)
        : base (analytics) {
            Title = "Select Language";
            Fuck = "fuckoff";
            ToolbarItems = new List<ToolbarItem>();
            var converter = new FileImageSourceConverter();
            ToolbarItems.Add(new ToolbarItem() {Text = "asd"});
        }
    }
}
