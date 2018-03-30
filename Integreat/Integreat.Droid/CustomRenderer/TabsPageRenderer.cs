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
        private ViewPager _viewPager;
        private TabLayout _tabLayout;

        public TabsPageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            for (var i = 0; i < ChildCount; i++)
            {
                var v = GetChildAt(i);
                if (v is ViewPager pager)
                    _viewPager = pager;
                else
                {
                    if (v is TabLayout layout)
                        _tabLayout = layout;
                }
            }
            _viewPager.SetPageTransformer(true, new NoAnimationPageTransformer());
        }
    }

    public class NoAnimationPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        public void TransformPage(Android.Views.View page, float position)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (position < 0)
            {
                page.ScrollX = (int)(page.Width * position);
            }
            else if (position > 0)
            {
                page.ScrollX = -(int)(page.Width * -position);
            }
            else
            {
                page.ScrollX = 0;
            }
        }
    }
}
