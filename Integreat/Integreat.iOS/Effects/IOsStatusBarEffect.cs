using Integreat.iOS.CustomRenderer;
using Integreat.Shared.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(IosStatusBarEffect), "StatusBarEffect")]
namespace Integreat.iOS.CustomRenderer
{
    /// <summary>
    /// This class is used to render the status bar effect 
    /// </summary>
    public class IosStatusBarEffect:PlatformEffect
    {
        protected override void OnAttached()
        {
            UIView statusBar = UIApplication.SharedApplication.ValueForKey(new Foundation.NSString("statusBar")) as UIView;
            if(statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = StatusBarEffect.GetBackgroundColor().ToUIColor();
            }
        }

        protected override void OnDetached(){}
    }
}
