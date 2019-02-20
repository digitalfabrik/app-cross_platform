using Xamarin.Forms;

namespace App1.Effects
{
    /// <inheritdoc />
    /// <summary>
    /// This class is for storing the Status-bar background color
    /// </summary>
    public class StatusBarEffect : RoutingEffect
    {
        private static Color _backgroundColor;

        /// <inheritdoc />
        public StatusBarEffect() : base("Integreat.StatusBarEffect") { }

        /// <summary> Sets the color of the background. </summary>
        /// <param name="color">The color.</param>
        public static void SetBackgroundColor(Color color) => _backgroundColor = color;

        /// <summary> Gets the color of the background. </summary>
        /// <returns></returns>
        public static Color GetBackgroundColor() => _backgroundColor;
    }
}
