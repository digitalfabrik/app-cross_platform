using System;
using CoreGraphics;
using Integreat.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace Integreat.iOS.CustomRenderer
{
    /// <summary>
    /// this class renders the tabbedPage
    /// </summary>
    public class TabbedPageRenderer : TabbedRenderer
    {
        private UISegmentedControl _segmentedControl;
        private UITabBar _tabBar;
        bool _isHidden;

        public TabbedPageRenderer(){
            _segmentedControl = new UISegmentedControl();
        }
        /*
        public override void ViewDidLayoutSubviews()
        {
            if (TabBar?.Items == null)
                return;

            if(!TabBar.Equals(_tabBar)){
                this._tabBar = TabBar;
                CreateSegmentedControl();
            }
            CreateSegmentedControl();
            base.ViewDidLayoutSubviews();
        }
        */

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            HideTabBar();
        }

        private void CreateSegmentedControl(){
            _segmentedControl.RemoveAllSegments();
            var tabs = Element as TabbedPage;
            if (tabs != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    var item = TabBar.Items[i];
                    _segmentedControl.InsertSegment(item.Title, i, false);

                }

                this.NativeView.Add(_segmentedControl);
            }
        }

        public void HideTabBar()
        {
            if (_isHidden)
                return;

            var screenRect = UIScreen.MainScreen.Bounds;
            var height = screenRect.Height;

            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft
                || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                height = screenRect.Width;
            }

            UIView.BeginAnimations(null);
            UIView.SetAnimationDuration(0.4);

            foreach (UIView view in this.View.Subviews)
            {
                if (view is UITabBar)
                    view.Frame = new CGRect(view.Frame.X, height, view.Frame.Width, view.Frame.Height);
                else
                {
                    view.Frame = new CGRect(view.Frame.X, view.Frame.Y, view.Frame.Width, height);
                    view.BackgroundColor = UIColor.Black;
                }
            }

            UIView.CommitAnimations();

            _isHidden = true;
        }

        public void ShowTabBar()
        {
            if (!_isHidden)
                return;

            var screenRect = UIScreen.MainScreen.Bounds;
            var height = screenRect.Height - 49f;

            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft
                || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                height = screenRect.Width - 49f;
            }

            UIView.BeginAnimations(null);
            UIView.SetAnimationDuration(0.4);

            foreach (UIView view in this.View.Subviews)
            {
                if (view is UITabBar)
                    view.Frame = new CGRect(view.Frame.X, height, view.Frame.Width, view.Frame.Height);
                else
                    view.Frame = new CGRect(view.Frame.X, view.Frame.Y, view.Frame.Width, height);
            }

            UIView.CommitAnimations();

            _isHidden = false;
        }
    }
}
