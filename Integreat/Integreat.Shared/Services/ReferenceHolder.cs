using System;
using System.Collections.Generic;
using System.Text;
using Integreat.Shared.ViewModels;

namespace Integreat.Shared.Services
{
    public class ReferenceHolder
    {
        public NavigationViewModel NavigationViewModel { get; set; }
        public PagesViewModel PagesViewModel { get; set; } 
        public TabViewModel TabViewModel { get; set; }
        public EventPagesViewModel EventPagesViewModel { get; set; }
    }
}
