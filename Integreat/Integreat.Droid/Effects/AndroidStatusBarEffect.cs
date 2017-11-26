using Integreat.Droid.Effects;
using Integreat.Droid.Helpers;
using Integreat.Shared.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Integreat")]
[assembly: ExportEffect(typeof(AndroidStatusBarEffect), "StatusBarEffect")]
namespace Integreat.Droid.Effects
{
    /// <summary>
    /// This Class is used to render the status bar effect 
    /// </summary>
    public class AndroidStatusBarEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var backgroundColor = StatusBarEffect.GetBackgroundColor().ToAndroid();
            var window = Globals.Window;
            window.SetStatusBarColor(backgroundColor);
        }

        protected override void OnDetached() { }
    }
}
