using Integreat.iOS.CustomRenderer;
using Integreat.Shared.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SpaceFittingLabel), typeof(SpaceFittingLabelRenderer))]
namespace Integreat.iOS.CustomRenderer
{
    /// <summary>
    /// Shrinks label to given size
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.iOS.LabelRenderer" />
    public class SpaceFittingLabelRenderer : LabelRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var lineSpacingLabel = (SpaceFittingLabel)Element;
            if (Control == null || lineSpacingLabel == null) return;
            Control.LineBreakMode = UILineBreakMode.TailTruncation;
            Control.AdjustsFontSizeToFitWidth = true;
            Control.MinimumFontSize = lineSpacingLabel.MinimalTextSize;
            Control.Lines = lineSpacingLabel.MaximalLineCount;
            Control.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
        }
    }
}
