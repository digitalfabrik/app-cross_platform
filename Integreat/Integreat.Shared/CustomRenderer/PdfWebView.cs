using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Integreat.Shared.CustomRenderer
{
    /// <summary>
    /// Special WebView custom control, that allows the viewing of local pdfs on android and windows (iOS works anyway)
    /// </summary>
    public class PdfWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(PdfWebView),
            defaultValue: default(string));

        public string Uri {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}
