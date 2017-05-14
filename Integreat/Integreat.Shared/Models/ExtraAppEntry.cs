using System;
using Integreat.Shared.ViewFactory;
using Xamarin.Forms;

namespace Integreat.Shared.Models
{
    /// <summary>
    /// Describes a extra entry, which are additional third party features.
    /// </summary>
    public class ExtraAppEntry
    {
        public string Title { get; set; }
        
        public string Thumbnail { get; set; }

        public Command OnTapCommand { get; set; }

        public Func<IViewModel> ViewModelFactory { get; set; }
    }
}
