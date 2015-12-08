using Xamarin.Forms;
using Page = Integreat.Models.Page;

namespace Integreat.Shared.Views
{
    public class WebsiteView : BaseView
    {
        public WebsiteView(Page page)
        {
            Title = page.Title;
            var webView = new WebView
            {
                Source = new HtmlWebViewSource
                {
                    Html = page.Content
                }
            };
            Content = webView;
        }
    }
}
