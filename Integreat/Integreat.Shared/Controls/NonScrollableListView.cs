using Xamarin.Forms;

namespace Integreat.Shared.Controls
{
    /// <inheritdoc />
    public class NonScrollableListView: ListView
    {
        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
        }
    }
}
