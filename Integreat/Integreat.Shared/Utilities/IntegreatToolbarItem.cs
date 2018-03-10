using System;
using Xamarin.Forms;

namespace Integreat.Shared.Utilities
{
    public class IntegreatToolbarItem : ToolbarItem
    {
        private string _identifier;

        public string Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
            }
        }


    }
}
