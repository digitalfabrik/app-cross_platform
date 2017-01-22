using Integreat.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Integreat")]
[assembly: ExportEffect(typeof(NoScrollListViewEffect), "NoScrollEffect")]
namespace Integreat.iOS.Effects {
    public class NoScrollListViewEffect : PlatformEffect {
        private UITableView _NativeList => Control as UITableView;

        protected override void OnAttached() {
            if (_NativeList != null) {
                _NativeList.ScrollEnabled = false;
            }
        }

        protected override void OnDetached() {
        }
    }
}