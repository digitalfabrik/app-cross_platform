
using Integreat.Droid.CustomRenderer;
using Integreat.Shared.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SpaceFittingLabel), typeof(SpaceFittingLabelRenderer))]
namespace Integreat.Droid.CustomRenderer {
    public class SpaceFittingLabelRenderer : EntryRenderer {
    }
}