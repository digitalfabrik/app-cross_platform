using System;
using Xamarin.Forms;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// Enables different gestures which allows to zoom in imGES
    /// based on answer of: https://forums.xamarin.com/discussion/74168/full-screen-image-viewer-with-pinch-to-zoom-pan-to-move-tap-to-show-captions-for-xamarin-forms
    /// </summary>
    public class ImageZoomContainer : ContentView
    {
        private const double MinScale = 1;
        private const double MaxScale = 4;
        private const double Overshoot = 0;
        private double _startScale;
        private double _lastX, _lastY;

        public ImageZoomContainer()
        {
            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);

            Scale = MinScale;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }

        /// <inheritdoc />
        /// <summary> Called when [measure]. </summary>
        /// <param name="widthConstraint">The width constraint.</param>
        /// <param name="heightConstraint">The height constraint.</param>
        /// <returns></returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = MinScale;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        /// <summary> Called when [tapped]. </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTapped(object sender, EventArgs e)
        {
            if (Scale > MinScale)
            {
                this.ScaleTo(MinScale, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5;
                this.ScaleTo(MaxScale, 250, Easing.CubicInOut);
            }
        }

        /// <summary> Called when [pan updated]. </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (!(Scale > MinScale)) return;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _lastX = TranslationX;
                    _lastY = TranslationY;
                    break;
                case GestureStatus.Running:
                    TranslationX = Extensions.Clamp(_lastX + e.TotalX * Scale, Width / 2, -Width / 2 );
                    TranslationY = Extensions.Clamp(_lastY + e.TotalY * Scale,Height / 2, -Height / 2);
                    break;
            }
        }

        /// <summary> Called when [pinch updated]. </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PinchGestureUpdatedEventArgs"/> instance containing the event data.</param>
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    _startScale = Scale;
                    AnchorX = e.ScaleOrigin.X;
                    AnchorY = e.ScaleOrigin.Y;
                    break;
                case GestureStatus.Running:
                    double current = Scale + (e.Scale - 1) * _startScale;
                    Scale = Extensions.Clamp(current, MaxScale * (1 + Overshoot), MinScale * (1 - Overshoot));
                    break;
                case GestureStatus.Completed:
                    if (Scale > MaxScale)
                        this.ScaleTo(MaxScale, 250, Easing.SpringOut);
                    else if (Scale < MinScale)
                        this.ScaleTo(MinScale, 250, Easing.SpringOut);
                    break;
            }
        }
    }
}