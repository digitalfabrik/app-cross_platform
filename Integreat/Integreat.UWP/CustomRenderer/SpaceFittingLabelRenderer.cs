using Integreat.Shared.CustomRenderer;
using Integreat.UWP.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SpaceFittingLabel), typeof(SpaceFittingLabelRenderer))]
namespace Integreat.UWP.CustomRenderer {
    public class SpaceFittingLabelRenderer : LabelRenderer {
        protected override void OnElementChanged(
           ElementChangedEventArgs<Label> e) {
            base.OnElementChanged(e);

            var lineSpacingLabel = (SpaceFittingLabel)Element;
            if (Control == null || lineSpacingLabel == null) return;
        }
    }
}
