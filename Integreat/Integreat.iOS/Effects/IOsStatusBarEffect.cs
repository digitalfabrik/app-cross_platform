using Integreat.iOS.Effects;
using Integreat.Shared.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(IosStatusBarEffect), "StatusBarEffect")]
namespace Integreat.iOS.Effects
{
    /// <summary>
    /// This class is used to render the Status-Bar effect
    /// </summary>
    public class IosStatusBarEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (UIApplication.SharedApplication.ValueForKey(new Foundation.NSString("statusBar"))
                is UIView statusBar
                && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = StatusBarEffect.GetBackgroundColor().ToUIColor();
            }
        }

        protected override void OnDetached() { }
    }
}
