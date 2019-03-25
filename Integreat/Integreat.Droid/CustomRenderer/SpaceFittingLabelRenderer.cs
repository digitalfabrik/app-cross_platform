using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Widget;
using Integreat.Droid.CustomRenderer;
using Integreat.Shared.CustomRenderer;
using Java.Lang;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Math = System.Math;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;

[assembly: ExportRenderer(typeof(SpaceFittingLabel), typeof(SpaceFittingLabelRenderer))]
namespace Integreat.Droid.CustomRenderer
{

    /// <summary>
    /// Fits a Label to a given size by shrinking the font size
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.Android.LabelRenderer" />
    public class SpaceFittingLabelRenderer : LabelRenderer
    {
        public SpaceFittingLabelRenderer(Context context) : base(context)
        {

        }
        // Minimum text size for this text view
        private float _minTextSize = 50;

        // Flag for text and/or size changes to force a resize
        private bool _needsResizing;

        // Text size that is set from code. This acts as a starting point for resizing
        private float _textSize;

        // Temporary upper bounds on the starting text size
        private float _maxTextSize;

        // Text view line spacing multiplier
        private const float SpacingMultiplier = 1.0f;

        // Text view additional line spacing
        private const float SpacingAdd = 0.0f;

        private int _maxLines = 100;

        /// <summary>
        /// Called when [element property changed]. Acts as the entry point for this custom control. When this method is called,
        /// the view has been constructed and Control is no longer null.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control == null) return;

            // Get the element as SpaceFittingLabel to access the specific properties
            if (Element is SpaceFittingLabel spaceFittingLabel)
            {
                _maxLines = spaceFittingLabel.MaximalLineCount;
                // convert the device independent pixel to pixel (as the algorithm below works with raw pixel)
                _maxTextSize = TypedValue.ApplyDimension(ComplexUnitType.Dip, spaceFittingLabel.MaximalTextSize, Context.Resources.DisplayMetrics);
                _minTextSize = TypedValue.ApplyDimension(ComplexUnitType.Dip, spaceFittingLabel.MinimalTextSize, Context.Resources.DisplayMetrics);
            }

            // Whenever the text is changed, we want to resize. Therefore register the event handler
            Control.TextChanged += ControlOnTextChanged;

            // init variables
            _textSize = Control.TextSize;
            _needsResizing = true;
            ResetTextSize();

            base.OnElementPropertyChanged(sender, e);
        }

        /// <summary>
        /// Resets the size of the text to the last set size.
        /// </summary>
        private void ResetTextSize()
        {
            if (_textSize <= 0) return;

            SetTextSize(_textSize);
            _maxTextSize = _textSize;
        }

        /// <summary>
        /// Sets the size of the text with raw pixel as scale.
        /// </summary>
        /// <param name="to">The value to set to.</param>
        private void SetTextSize(float to)
        {
            // Use raw pixel so the value is stable, as the setter for Control.TextSize
            // uses dependent calculations to abstract the device dpi from the developer.
            Control.SetTextSize(ComplexUnitType.Px, to);
            _textSize = to;
        }

        /// <summary>
        /// Called when the [layout] changes and this control shall adapt to it.
        /// </summary>
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            // if the layout has changed or the needsResize flag is set, we want to calculate our available width/height and execute the resize method
            if (changed || _needsResizing)
            {
                var widthLimit = (r - l) - Control.CompoundPaddingLeft - Control.CompoundPaddingRight;
                var heightLimit = (b - t) - Control.CompoundPaddingBottom - Control.CompoundPaddingTop;
                ResizeText(widthLimit, heightLimit);
            }
            base.OnLayout(changed, l, t, r, b);
        }

        private void ControlOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            // whenever the text was changed, get the available width/height and execute the resize method
            var heightLimit = Height - PaddingBottom - PaddingTop;
            var widthLimit = Width - PaddingLeft - PaddingRight;
            ResizeText(widthLimit, heightLimit);
        }



        /// <summary>
        /// Resizes the text for the given width/height.
        /// </summary>
        /// <param name="width">The available width in pixel.</param>
        /// <param name="height">The available height in pixel.</param>
        private void ResizeText(int width, int height)
        {
            // cast the c# string into a Java CharSequence so it works with the native methods
            var text = CharSequence.ArrayFromStringArray(new[] { Control.Text })[0];
            var end = text.Length();
            // Do not resize if the view does not have dimensions or there is no text
            if (text.Length() == 0 || height <= 0 || width <= 0 || Math.Abs(_textSize) < 0.1)
            {
                return;
            }

            if (Control.TransformationMethod != null)
            {
                text = Control.TransformationMethod.GetTransformationFormatted(text, this);
                end = text.Length();
            }

            // Get the text view's paint object
            var textPaint = Control.Paint;

            // If there is a max text size set, use the lesser of that and the default text size
            var targetTextSize = _maxTextSize > 0 ? Math.Min(_textSize, _maxTextSize) : _textSize;

            // Get the maximal text height
            var layout = GetTextLayout(text, end, textPaint, width, targetTextSize);

            // Until we either fit within our text view or we had reached our min text size, incrementally try smaller sizes
            while ((layout.Height > height || layout.LineCount > _maxLines) && targetTextSize > _minTextSize)
            {
                targetTextSize = Math.Max(targetTextSize - 2, _minTextSize);
                layout = GetTextLayout(text, end, textPaint, width, targetTextSize);
            }

            // If we had reached our minimum text size and still don't fit, append an ellipsis
            if (Math.Abs(targetTextSize - _minTextSize) < 1 && layout.Height > height)
            {
                targetTextSize = _minTextSize;
                var newText = CharSequence.ArrayFromStringArray(new[] { text.SubSequence(0, end) + "..." })[0];
                end = newText.Length();
                layout = GetTextLayout(newText, end, textPaint, width, targetTextSize);

                // Until we either fit within our text view or we had reached our min text size, incrementally try smaller sizes
                while (layout.Height > height || layout.LineCount > _maxLines)
                {
                    end -= 1;
                    if (end < 1) break;
                    newText = CharSequence.ArrayFromStringArray(new[] { text.SubSequence(0, end) + "..." })[0];
                    end = newText.Length();
                    layout = GetTextLayout(newText, end, textPaint, width, targetTextSize);
                }

                Control.SetText(text.SubSequence(0, end) + "...", TextView.BufferType.Normal);
            }

            // finally set the text size we calculated
            SetTextSize(targetTextSize);

            // Reset force resize flag
            _needsResizing = false;
        }

        /// <summary>
        /// Set the text size of the text paint object and use a static layout to render text off screen before measuring and returns it's height.
        /// </summary>
        /// <param name="source">The text source.</param>
        /// <param name="end"></param>
        /// <param name="paint">The paint object that will be used to copy the style.</param>
        /// <param name="width">The available width to render the text.</param>
        /// <param name="textSize">Size of the text.</param>
        /// <param name="includePad">if set to <c>true</c> [the padding is included] (in the static layout).</param>
        /// <returns>
        /// Static layout containing a new text with the given parameter.
        /// </returns>
        private static StaticLayout GetTextLayout(
            ICharSequence source, int end, Paint paint, int width, float textSize, bool includePad = true)
        {
            // modified: make a copy of the original TextPaint object for measuring
            // (apparently the object gets modified while measuring, see also the
            // docs for TextView.getPaint() (which states to access it read-only)
            var paintCopy = new TextPaint(paint) { TextSize = textSize };

            // Measure using a static layout

            var builder = StaticLayout.Builder.Obtain(source, 0, end, paintCopy, width)
                .SetAlignment(Android.Text.Layout.Alignment.AlignNormal)
                .SetLineSpacing(SpacingAdd, SpacingMultiplier)
                .SetIncludePad(includePad);
            var layout = builder.Build();
            return layout;
        }

    }
}
