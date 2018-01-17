using System;
using System.Linq;
using Integreat.iOS.CustomRenderer;
using Integreat.Shared.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MainNavigationPage), typeof(MainNavigationPageRenderer))]
namespace Integreat.iOS.CustomRenderer
{
    /// <summary>
    /// A custom renderer for the MainNavigationPage
    /// </summary>
    public class MainNavigationPageRenderer : NavigationRenderer
    {
  
        public override void ViewWillAppear(bool animated)
        {
            var toolbar = View.Subviews.OfType<UIToolbar>().FirstOrDefault(v => v.GetType() != typeof(UIToolbar));
            if(toolbar != null){
                toolbar.RemoveFromSuperview();
            }
            base.ViewWillAppear(animated);
        }
    }
}
