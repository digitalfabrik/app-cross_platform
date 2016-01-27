using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Integreat.Shared.ViewModels;
using Integreat.Shared.Views;
using System.Collections.ObjectModel;

using Xamarin.Forms;
namespace Integreat.Shared.Pages
{
    public partial class PageSearchList : ContentPage
    {
        private PageSearch search;
        private SearchViewModel viewModel;

        public PageSearchList(PagesViewModel pagesViewModel)
        {
            InitializeComponent();
            search = new PageSearch("Page");
            viewModel = new SearchViewModel(search, pagesViewModel);

            viewModel.SearchCompleted += (sender, e) =>
            {
                if (viewModel.Pages == null)
                {
                    listView.ItemsSource = new Collection<Models.Page>();
                }
                else
                {
                    listView.ItemsSource = viewModel.Pages;
                }
            };

            viewModel.Error += (sender, e) =>
            {
                DisplayAlert("Error", e.Exception.Message, "OK", null);
            };

            BindingContext = viewModel;
        }

        private void OnValueChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.Search();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                return;
            }
            var page = listView.SelectedItem as Models.Page;
            Navigation.PushAsync(new WebsiteView(page));
            listView.SelectedItem = null;
        }
    }
}
