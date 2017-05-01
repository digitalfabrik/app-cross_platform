using System;
using Integreat.Shared.ViewFactory;
using Xamarin.Forms;

namespace Integreat.Shared.Models
{
    
    public class ExtraAppEntry
    {
        public string Title { get; set; }
        
        public string Thumbnail { get; set; }

        public Command OnTapCommand { get; set; }

        public Func<IViewModel> ViewModelFactory { get; set; }
    }
}
