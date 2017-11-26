using System;
using Xamarin.Forms;

namespace Integreat.Shared.Effects
{
    public class StatusBarEffect : RoutingEffect
    {
        private static Color _backgroundColor;

        public StatusBarEffect() : base("Xamarin.StatusBarEffect") { }

        public static Color BackgroundColor { get => _backgroundColor; set => _backgroundColor = value; }
    }
}
