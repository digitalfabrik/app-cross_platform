using System;
using Android.Content;
using Integreat.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabsPageRenderer))]
namespace Integreat.Droid.CustomRenderer
{
    public class TabsPageRenderer : TabbedPageRenderer
    {

        ViewPager _viewPager;
        TabLayout _tabLayout;

        public TabsPageRenderer(Context context):base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            for (int i = 0; i < ChildCount; i++)
            {
                var v = GetChildAt(i);
                if (v is ViewPager)
                    _viewPager = (ViewPager)v;
                else if (v is TabLayout)
                    _tabLayout = (TabLayout)v;
            }

            _viewPager.SetPageTransformer(true, new NoAnimationPageTransformer());
        }
    }

    public class NoAnimationPageTransformer : Java.Lang.Object, Android.Support.V4.View.ViewPager.IPageTransformer
    {
        public void TransformPage(Android.Views.View view, float position)
        {
            if (position < 0)
            {
                view.ScrollX = (int)((float)(view.Width) * position);
            }
            else if (position > 0)
            {
                view.ScrollX = -(int)((float)(view.Width) * -position);
            }
            else
            {
                view.ScrollX = 0;
            }

        }
    }
}
