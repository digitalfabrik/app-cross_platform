using Android.Views;
using Integreat.Droid.Helpers;
using Integreat.Shared.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(StatusBarEffect), "StatusBarEffect")]
namespace Integreat.Droid.CustomRenderer
{
    /// <summary>
    /// This Class is used to render the status bar effect 
    /// </summary>
    public class StatusBarEffectRenderer : PlatformEffect
    {
        protected override void OnAttached()
        {
            var backgroundColor = StatusBarEffect.BackgroundColor.ToAndroid();
            Window window = Globals.Window;
            window.SetStatusBarColor(backgroundColor);
        }

        protected override void OnDetached(){}
    }
}
