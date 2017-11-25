using Integreat.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Integreat")]
[assembly: ExportEffect(typeof(NoScrollListViewEffect), "NoScrollEffect")]
namespace Integreat.iOS.Effects
{
    public class NoScrollListViewEffect : PlatformEffect
    {
        private UITableView NativeList => Control as UITableView;

        protected override void OnAttached()
        {
            if (NativeList != null)
            {
                NativeList.ScrollEnabled = false;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}