using App1.Utilities;
using App1.ViewModels;
using Xamarin.Forms;

namespace App1.Pages
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

            Icon = root.Icon;
            Title = root.Title;

            //add toolbaritems
            var toolbarItems = ((BaseContentViewModel)root.BindingContext).ToolbarItems;
            if(toolbarItems != null)
            {
                ToolbarItems.AddRange(toolbarItems);   
            }
        }
    }
}
