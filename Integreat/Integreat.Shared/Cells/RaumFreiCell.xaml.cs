using System.Windows.Input;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras.Raumfrei;
using Integreat.Shared.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Integreat.Shared.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class RaumFreiCell : ViewCell
    {
        private readonly INavigation _navigation;
        public RaumFreiCell(INavigation navigation = null)
        {
            Height = 120;
            _navigation = navigation;
            View = new RaumFreiCellView();
        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (_navigation == null)
                return;
            var raumFrei = BindingContext as RaumfreiOffer;
            if (raumFrei == null)return;

            IntegreatApp.Logger.TrackPage(AppPage.Extras.ToString(), raumFrei.EmailAddress);
            //await Navigator.PushAsync(_navigation, new RaumFreiDetailsPage(raumFrei));
        }
    }

    public partial class RaumFreiCellView : ContentView
    {
        public RaumFreiCellView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty FavoriteCommandProperty =
            BindableProperty.Create(nameof(FavoriteCommand), typeof(ICommand), typeof(RaumFreiCellView), default(ICommand));

        public ICommand FavoriteCommand
        {
            get => GetValue(FavoriteCommandProperty) as Command;
            set => SetValue(FavoriteCommandProperty, value);
        }
    }
}