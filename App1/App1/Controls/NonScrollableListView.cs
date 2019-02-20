using Xamarin.Forms;

namespace App1.Controls
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
