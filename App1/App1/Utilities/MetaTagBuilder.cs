using System.Collections.Generic;
using System.Linq;

namespace App1.Utilities
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
        private string Content { get; }

        public IEnumerable<string> MetaTags { get; }

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
