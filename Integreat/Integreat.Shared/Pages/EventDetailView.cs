using Integreat.Models;
using Integreat.Shared.Views;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public class EventDetailView : BaseView
    {
        public EventDetailView(EventPage item)
        {
            BindingContext = item;
            var webView = new WebView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Source = new HtmlWebViewSource
                {
                    Html = item.Content
                }
            };
            Content = new StackLayout
            {
                Children =
                {
                    webView
                }
            };
            /*var share = new ToolbarItem
            {
                Icon = "ic_share.png",
                Text = "Share",
                Command = new Command(() => DependencyService.Get<IShare>()
                    .ShareText("Be sure to read @shanselman's " + item.Title + " " + item.Link))
            };

            ToolbarItems.Add(share);*/
        }
    }
}
