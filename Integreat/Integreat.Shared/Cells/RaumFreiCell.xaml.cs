using System;
using System.Windows.Input;
using Integreat.Model;
using Integreat.Model.Extras.Raumfrei;
using Integreat.Shared.Services;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Integreat.Shared.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class RaumFreiCell : ViewCell
    {
        private readonly INavigation _navigation;
        private readonly INavigator _navigator;
        private readonly Func<RaumfreiOffer, RaumfreiDetailViewModel> _raumfreiDetailPageFactory;

        public RaumFreiCell()
        {

        }
        public RaumFreiCell(Func<RaumfreiOffer, RaumfreiDetailViewModel> raumfreiDetailPageFactory, INavigator navigator, INavigation navigation = null)
        {
            Height = 120;
            _raumfreiDetailPageFactory = raumfreiDetailPageFactory;
            _navigator = navigator;
            _navigation = navigation;
            View = new RaumFreiCellView();
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(RaumFreiCellView), default(string));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty RoomCountProperty =
            BindableProperty.Create(nameof(RoomCount), typeof(string), typeof(RaumFreiCellView), default(string));

        public string RoomCount
        {
            get => (string)GetValue(RoomCountProperty);
            set => SetValue(RoomCountProperty, value);
        }

        public static readonly BindableProperty MoveInDateProperty =
            BindableProperty.Create(nameof(MoveInDate), typeof(string), typeof(RaumFreiCellView), default(string));

        public string MoveInDate
        {
            get => (string)GetValue(MoveInDateProperty);
            set => SetValue(MoveInDateProperty, value);
        }

        public static readonly BindableProperty RentalCostCompleteProperty =
            BindableProperty.Create(nameof(RentalCostComplete), typeof(string), typeof(RaumFreiCellView), default(string));

        public string RentalCostComplete
        {
            get => (string)GetValue(RentalCostCompleteProperty);
            set => SetValue(RentalCostCompleteProperty, value);
        }

        protected override async void OnTapped()
        {
            base.OnTapped();
            if (_navigation == null)
                return;
            if (!(BindingContext is RaumfreiOffer raumFrei)) return;

            IntegreatApp.Logger.TrackPage(AppPage.Extras.ToString(), raumFrei.EmailAddress);
            var page = _raumfreiDetailPageFactory(raumFrei);
            await _navigator.PushAsync(page, _navigation);
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