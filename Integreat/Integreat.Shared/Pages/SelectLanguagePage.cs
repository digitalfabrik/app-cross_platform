using System;
using Integreat.Models;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;
using Integreat.Shared.Utilities;
using Integreat.Shared.Pages;

namespace Integreat.Shared
{
    public class SelectLanguagePage : ContentPage
    {
        private Location location;

        public SelectLanguagePage(Location location)
        {
            this.location = location;
            Title = "Select Language";

            BindingContext = new LanguagesViewModel(location);

            var flowListView = new FlowListView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FlowColumnsDefinitions = new List<Func<object, Type>>() {
                    new Func<object, Type> ((bindingContext) => typeof(LocationViewCell)),
                    new Func<object, Type> ((bindingContext) => typeof(LocationViewCell)),
                    new Func<object, Type> ((bindingContext) => typeof(LocationViewCell)),
                },
                SeparatorVisibility = SeparatorVisibility.Default
            };
            flowListView.SetBinding<LanguagesViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Items, BindingMode.TwoWay);
            flowListView.ItemSelected += (sender, e) => {
                flowListView.SelectedItem = null;
            };


            flowListView.FlowItemTapped += (sender, e) => {
                var item = e.Item as Language;
                if (item != null)
                {
                    System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Name);
                    Preferences.SetLanguage(location, item);
                    Navigation.PushAsync(new RootPage());
                }
            };

            Content = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    new Label () { Text = "What language do you speak?" },
                    new Entry (),
                    flowListView,
                }
            };
        }

        class LocationViewCell : FlowStackCell
        {
            public LocationViewCell()
            {
                var label = new Label()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                };
                label.SetBinding<Location>(Label.TextProperty, l => l.Name, BindingMode.TwoWay);

                Orientation = StackOrientation.Vertical;
                var cityImage = new Image();
                cityImage.SetBinding<Location>(Image.SourceProperty, l => l.CityImage, BindingMode.TwoWay);

                RelativeLayout layout = new RelativeLayout();

                layout.Children.Add(cityImage,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => {
                        return parent.Width;
                    }),
                    Constraint.RelativeToParent((parent) => {
                        return parent.Height;
                    }));

                layout.Children.Add(label,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => {
                        return parent.Width;
                    }),
                    Constraint.RelativeToParent((parent) => {
                        return parent.Height;
                    }));

                //				Children.Add (layout);
                Children.Add(cityImage);

                HeightRequest = 300;
                WidthRequest = 300;
            }
        }
    }

}

