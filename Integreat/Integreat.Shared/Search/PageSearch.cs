using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Integreat.Shared.Models
{
    public class PageSearch
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public IEnumerable<Page> Results { get; set; }

        public PageSearch()
            : this("")
        {
        }

        public PageSearch(string name)
        {
            Name = name;
            Text = "";
            Results = new Collection<Page>();
        }

    }
}
