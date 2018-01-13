using Xamarin.Forms;

namespace Integreat.Shared.Effects
{
    /// <summary>
    /// This class is for storing the Statusbar background color
    /// </summary>
    public class StatusBarEffect : RoutingEffect
    {
        private static Color _backgroundColor;

        public StatusBarEffect() : base("Integreat.StatusBarEffect") { }

        public static void SetBackgroundColor(Color color)
        {
            _backgroundColor = color; 
        }

        public static Color GetBackgroundColor()
        {
            return _backgroundColor;
        }
    }
}
