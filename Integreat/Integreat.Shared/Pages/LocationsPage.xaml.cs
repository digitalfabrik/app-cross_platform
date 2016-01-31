using System;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;
using Integreat.Shared.Models;

namespace Integreat.Shared.Pages
{
    public partial class LocationsPage : ContentPage
    {
        public LocationsPage()
        {
            Title = "Select Location";
            var flowListView = new FlowListView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FlowColumnsDefinitions = new List<Func<object, Type>> {
                    bindingContext => typeof(LocationViewCell),
                    bindingContext => typeof(LocationViewCell),
                    bindingContext => typeof(LocationViewCell),
                },
                SeparatorVisibility = SeparatorVisibility.Default
            };
            flowListView.SetBinding<LocationsViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Items, BindingMode.TwoWay);
            flowListView.ItemSelected += (sender, e) => {
                flowListView.SelectedItem = null;
            };


            flowListView.FlowItemTapped += (sender, e) => {
                var item = e.Item as Location;
                if (item != null)
                    System.Diagnostics.Debug.WriteLine("FlowListView tapped: {0}", item.Name);
            };

            Content = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    new Label () { Text = "Where are you?" },
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
