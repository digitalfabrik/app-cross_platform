using Xamarin.Forms;

namespace Integreat.Shared.Views
{
	public partial class MainMenu
    {
        public ListView ListView => listView;

	    public MainMenu ()
		{
			InitializeComponent ();
        }
        
        public class NavigationItem
        {
            public string IconSource { get; set; }
            public string Title { get; set; }
            public int Id { get; set; }
        }
    }
}
