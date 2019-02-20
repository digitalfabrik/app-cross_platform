using Xamarin.Forms;

namespace App1.Effects
{
    /// <summary>
    /// Effect for ListView to disable the Scrollbar on iOS. This is due the fact, that iOS enables a scrolling method even though the listView is larger than the displayed content. (Overscrolling)
    /// </summary>
    public class NoScrollListViewEffect : RoutingEffect
    {
        public NoScrollListViewEffect() : base("Integreat.NoScrollEffect")
        {
        }
    }
}
