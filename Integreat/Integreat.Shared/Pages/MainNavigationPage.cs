using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    /// <inheritdoc />
    /// <summary>
    /// Main navigation page. This class is just a wrapper. So that the renderer doesn't have to render a xamarin base class
    /// </summary>
    public class MainNavigationPage : NavigationPage
    {
        public MainNavigationPage(Page root) : base(root)
        {
            //just for design purposes
            BarTextColor = (Color)Application.Current.Resources["TextColor"];
            BackgroundColor = (Color)Application.Current.Resources["HighlightColor"];

            this.Icon = root.Icon;
            this.Title = root.Title;
        }
    }
}

