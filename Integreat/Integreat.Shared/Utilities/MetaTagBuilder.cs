using System;
using System.Collections.Generic;
using Integreat.Utilities;

namespace Integreat.Shared.Utilities
{
    public class MetaTagBuilder
    {
        //build a full html page
        private string _content;
        private List<string> _metaTags = new List<string>();

        public string Content { get => _content; set => _content = value; }
        public List<string> MetaTags { get => _metaTags; set => _metaTags = value; }

        public MetaTagBuilder(List<string> metaTags, string content)
        {
            MetaTags = metaTags;
            Content = content;
        }

        public MetaTagBuilder(){
            
        }

        public string Build()
        {
            return HtmlTags.Doctype.GetStringValue() + Constants.MetaTagBuilderTag + HtmlTags.HtmlOpen.GetStringValue()
                           +HtmlTags.HeadOpen.GetStringValue()+String.Join("", MetaTags.ToArray())+HtmlTags.HeadClose.GetStringValue()
                           +HtmlTags.BodyOpen.GetStringValue()+Content+HtmlTags.BodyClose.GetStringValue()+HtmlTags.HtmlClose.GetStringValue();
        }
    }
}
