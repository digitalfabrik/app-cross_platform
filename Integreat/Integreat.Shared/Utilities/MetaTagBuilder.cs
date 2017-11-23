using System.Collections.Generic;
using Integreat.Utilities;

namespace Integreat.Shared.Utilities
{
    /// <summary>
    /// The MetaTagBuilder generate a html page with variable meta data in their head 
    /// and put the page content in the body.
    /// </summary>
    public class MetaTagBuilder
    {
        //build a full html page

        /// <summary> Gets or sets the content which will be inserted in the html body. </summary>
        /// <value> The content. </value>
        public string Content { private get; set; }

        public List<string> MetaTags { get; }

        public MetaTagBuilder(string content)
        {
            MetaTags = new List<string>();
            Content = content;
        }

        /// <summary>
        /// Builds the html page.
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            return HtmlTags.Doctype.GetStringValue()
                + Constants.MetaTagBuilderTag
                + HtmlTags.HtmlOpen.GetStringValue()
                + HtmlTags.HeadOpen.GetStringValue()
                + string.Join("", MetaTags.ToArray())
                + HtmlTags.HeadClose.GetStringValue()
                + HtmlTags.BodyOpen.GetStringValue()
                + Content 
                + HtmlTags.BodyClose.GetStringValue()
                + HtmlTags.HtmlClose.GetStringValue();
        }
    }
}
