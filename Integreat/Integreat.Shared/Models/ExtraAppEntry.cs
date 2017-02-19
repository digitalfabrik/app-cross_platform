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
        public string Name { get; set; }
        
        public string Image { get; set; }

        public Command OnTapCommand { get; set; }

        public Func<IViewModel> ViewModelFactory { get; set; }
    }
}
