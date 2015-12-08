using Xamarin.Forms;

namespace Integreat.Shared.Controls
{
    public class MyNavigationPage : NavigationPage
    {
        public MyNavigationPage(Page root) : base(root)
        {
            Init();
        }

        public MyNavigationPage()
        {
            Init();
        }

        private void Init()
        {
            BarBackgroundColor = Color.FromHex("#FFA000");
            BarTextColor = Color.White;
        }
    }
}
