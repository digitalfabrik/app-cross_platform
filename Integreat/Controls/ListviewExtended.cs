using System;
using Xamarin.Forms;

namespace Integreat
{
    public class ListViewExtended : ListView
    {
        #region IsScrollable
        public static readonly BindableProperty IsScrollableProperty =
            BindableProperty.Create<ListViewExtended, bool>(p => p.IsScrollable, true);

        public bool IsScrollable
        {
            get { return (bool)GetValue(IsScrollableProperty); }
            set { SetValue(IsScrollableProperty, value); }
        }
        #endregion
    }
}

