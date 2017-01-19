using Integreat.Shared.CustomRenderer;
using Integreat.UWP.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SpaceFittingLabel), typeof(SpaceFittingLabelRenderer))]
namespace Integreat.UWP.CustomRenderer {
    public class SpaceFittingLabelRenderer : LabelRenderer {
        public SpaceFittingLabelRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
        }
    }
}
