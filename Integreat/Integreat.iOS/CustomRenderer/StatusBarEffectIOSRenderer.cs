using Integreat.Shared.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(StatusBarEffect), "StatusBarEffect")]
namespace Integreat.iOS.CustomRenderer
{
    /// <summary>
    /// This Class is used to render the status bar effect 
    /// </summary>
    public class StatusBarEffectIOSRenderer:PlatformEffect
    {
        protected override void OnAttached()
        {
            UIView statusBar = UIApplication.SharedApplication.ValueForKey(new Foundation.NSString("statusBar")) as UIView;
            if(statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor")))
            {
                statusBar.BackgroundColor = StatusBarEffect.BackgroundColor.ToUIColor();
            }
        }

        protected override void OnDetached(){}
    }
}
