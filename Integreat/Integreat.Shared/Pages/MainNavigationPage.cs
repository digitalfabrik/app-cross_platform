using System;

using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    /// <summary>
    /// Main navigation page. This class is just a wrapper. So that the renderer doesn't have to render a xamarin base class
    /// </summary>
    public class MainNavigationPage : NavigationPage
    {
        public MainNavigationPage(Page root) : base(root){}
    }
}

