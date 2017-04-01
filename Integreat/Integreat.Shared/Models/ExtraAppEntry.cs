using System;
using System.Collections.Generic;
using System.Text;
using Integreat.Shared.ViewFactory;
using Integreat.Shared.ViewModels.Resdesign;
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
