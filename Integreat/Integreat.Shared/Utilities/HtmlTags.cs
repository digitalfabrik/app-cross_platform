using System;
namespace Integreat.Shared.Utilities
{
    public enum HtmlTags
    {
        //html tags 
        [StringValue("<!doctype html>")]
        Doctype = 1,
        [StringValue("<html>")]
        HtmlOpen = 2,
        [StringValue("</html>")]
        HtmlClose = 3,
        [StringValue("<head>")]
        HeadOpen = 4,
        [StringValue("</head>")]
        HeadClose = 5,
        [StringValue("<body>")]
        BodyOpen = 6,
        [StringValue("</body>")]
        BodyClose = 7
    }
}
