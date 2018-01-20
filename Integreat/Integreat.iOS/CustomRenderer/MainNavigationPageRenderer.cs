using System;
using System.Linq;
using Integreat.iOS.CustomRenderer;
using Integreat.Shared.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(MainNavigationPage), typeof(MainNavigationPageRenderer))]
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
            if (toolbar != null)
            {
                toolbar.RemoveFromSuperview();
            }
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            UIView[] subviews = View.Subviews.Where(v => v != NavigationBar).ToArray();
            var toolBarViews = subviews.Where(v => v is UIToolbar).ToArray();
            var otherViews = subviews.Where(v => !(v is UIToolbar)).ToArray();

            nfloat toolBarHeight = 0;

            foreach(var uiView in toolBarViews){
                uiView.SizeToFit();
                uiView.Frame = new CoreGraphics.CGRect
                {
                    X = 0,
                    Y = View.Bounds.Height - uiView.Frame.Height,
                    Width = View.Bounds.Width,
                    Height = uiView.Frame.Height
                };

                var thisToolBarHeight = uiView.Frame.Height;
                if(toolBarHeight < thisToolBarHeight){
                    toolBarHeight = thisToolBarHeight;
                }
            }

            var othersHeight = View.Bounds.Height - toolBarHeight;
            var othersFrame = new CoreGraphics.CGRect(View.Bounds.X, View.Bounds.Y, View.Bounds.Width, othersHeight);

            foreach(var uiView in otherViews){
                uiView.Frame = othersFrame;
            }
        }
    }
}
