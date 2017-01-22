using System;
using System.ComponentModel;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Widget;
using Integreat.Droid.CustomRenderer;
using Integreat.Shared.CustomRenderer;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Math = System.Math;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using System.Diagnostics;
using Android.Graphics;

[assembly: ExportRenderer(typeof(SpaceFittingLabel), typeof(SpaceFittingLabelRenderer))]
namespace Integreat.Droid.CustomRenderer {

    public class SpaceFittingLabelRenderer : LabelRenderer {


        // Minimum text size for this text view
        private static readonly float MinTextSize = 5;

        // Flag for text and/or size changes to force a resize
        private bool _needsResizing;

        // Text size that is set from code. This acts as a starting point for resizing
        private float _textSize;

        // Temporary upper bounds on the starting text size
        private float _maxTextSize;

        // Text view line spacing multiplier
        private float _spacingMult = 1.0f;

        // Text view additional line spacing
        private float _spacingAdd = 0.0f;

        private int _maxLines = 100;

        /// <summary>
        /// Called when [element property changed]. Acts as the entry point for this custom control. When this method is called, the view has been constructed and Control is no longer null.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control == null) return;

            // Get the element as SpaceFittingLabel to access the specific properties
            var spaceFittingLabel = Element as SpaceFittingLabel;
            if (spaceFittingLabel != null)
            {
                _maxLines = (int) spaceFittingLabel.MaximalLineCount;
                _maxTextSize = spaceFittingLabel.MaximalTextSize;
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
        private void ResetTextSize() {
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
            // Use raw pixel so the value is stable, as the setter for Control.TextSize uses dependent calculations to abstract the device dpi from the developer. So Control.TextSize = a; if(Control.TextSize == a) is not always true
            Control.SetTextSize(ComplexUnitType.Px, to);
            _textSize = to;
        }


        /// <summary>
        /// Called when the [layout] changes and this control shall adapt to it.
        /// </summary>
        protected override void OnLayout(bool changed, int l, int t, int r, int b) {
            // if the layout has changed or the needsResize flag is set, we want to calculate our available width/height and execute the resize method
            if (changed || _needsResizing) {
                var widthLimit = (r - l) - Control.CompoundPaddingLeft - Control.CompoundPaddingRight;
                var heightLimit = (b - t) - Control.CompoundPaddingBottom - Control.CompoundPaddingTop;
                ResizeText(widthLimit, heightLimit);
            }
            base.OnLayout(changed, l, t, r, b);
        }


        private void ControlOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs) {
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
        private void ResizeText(int width, int height) {
            // cast the c# string into a Java CharSequence so it works with the native methods
            var text = CharSequence.ArrayFromStringArray(new[] { Control.Text })[0];

            // Do not resize if the view does not have dimensions or there is no text
            if (text == null || text.Length() == 0 || height <= 0 || width <= 0 || Math.Abs(_textSize) < 0.1) {
                return;
            }

            if (Control.TransformationMethod != null) {
                text = Control.TransformationMethod.GetTransformationFormatted(text, this);
            }

            // Get the text view's paint object
            var textPaint = Control.Paint;
            
            // If there is a max text size set, use the lesser of that and the default text size
            var targetTextSize = _maxTextSize > 0 ? Math.Min(_textSize, _maxTextSize) : _textSize;

            // Get the maximal text height
            var layout = GetTextLayout(text, textPaint, width, targetTextSize);

            // Until we either fit within our text view or we had reached our min text size, incrementally try smaller sizes
            while (layout.Height > height && targetTextSize > MinTextSize || layout.LineCount > _maxLines) {
                targetTextSize = Math.Max(targetTextSize - 2, MinTextSize);
                layout = GetTextLayout(text, textPaint, width, targetTextSize);
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
        /// <param name="paint">The paint object that will be used to copy the style.</param>
        /// <param name="width">The available width to render the text.</param>
        /// <param name="textSize">Size of the text.</param>
        /// <returns>Static layout containing a new text with the given parameter.</returns>
        private StaticLayout GetTextLayout(ICharSequence source, Paint paint, int width, float textSize) {
            // modified: make a copy of the original TextPaint object for measuring
            // (apparently the object gets modified while measuring, see also the
            // docs for TextView.getPaint() (which states to access it read-only)
            var paintCopy = new TextPaint(paint) {TextSize = textSize};

            // Measure using a static layout
            return new StaticLayout(source, paintCopy, width, Android.Text.Layout.Alignment.AlignNormal, _spacingMult, _spacingAdd, true);
        }

    }
}
