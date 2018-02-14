using System;
using Xamarin.Forms;

namespace Integreat.Shared.Views
{
    /// <summary>
    /// Custom Map View
    /// </summary>
    public class MapsContentView:View
    {
        public static readonly BindableProperty IndeterminateProperty = BindableProperty.Create<MapsContentView, bool>(prop => prop.Changed, default(bool));

        public bool Changed
        {
            get { return (bool)GetValue(IndeterminateProperty); }
            set
            {
                SetValue(IndeterminateProperty, value);
                OnPropertyChanged();
            }
        }
    }
}
