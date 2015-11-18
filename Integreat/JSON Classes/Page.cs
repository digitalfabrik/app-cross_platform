using System;

namespace Integreat
{

    public class Autor {
        public string login { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class AvailableLanguages {
        public string code { get; set; }
        public int id { get; set; }
    }

    public class Page
    {
        public int id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string modified_gmt { get; set; }
        public string excerpt { get; set; }
        public string content { get; set; }
        public int parent { get; set; }
        public int order { get; set; }
        public object available_languages { get; set; }
        public string thumbnail { get; set; }
        public Autor author { get; set; }
    }
}

