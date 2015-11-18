using System;

using Xamarin.Forms;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;
using System.Collections.Generic;

namespace Integreat
{
    public class overview : SideBarPage
    {


        public static ViewCellExtended CreateCardViewTemplate()
        {
            var TitleItem = new Label
            {
                TextColor = Color.FromRgb(0,0,128),
                Text = "##########",
                FontAttributes = FontAttributes.Bold,
                FontSize = 13,
                    VerticalOptions = LayoutOptions.Fill
            };
            var DateItem = new Label
            {
                TextColor = Color.FromRgb(0,0,128),
                Text = "##########",
                FontAttributes = FontAttributes.Bold,
                FontSize = 13,
                XAlign = TextAlignment.End,
                VerticalOptions = LayoutOptions.End
            };
            var ExcerptItem = new Label
            {
                TextColor = Color.FromRgb(0,0,0),
                Text = "##########",
                FontSize = 11,
                    HeightRequest = 80,
            };
            TitleItem.SetBinding(Label.TextProperty, new Binding("title", stringFormat: "{0}"));
            DateItem.SetBinding(Label.TextProperty, new Binding("modified_gmt", stringFormat: "{0}"));
            ExcerptItem.SetBinding(Label.TextProperty, new Binding("excerpt", stringFormat: "{0}"));
            Frame F = new Frame();
            F.HeightRequest = 110;
            F.Padding = new Thickness(0);
            F.Content = new StackLayout {
                Padding = new Thickness(5),
                Children = {
                    new StackLayout {
                        Padding = new Thickness(0),
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            TitleItem,
                            DateItem
                        },
                        VerticalOptions = LayoutOptions.Fill
                    },
                    ExcerptItem
                }

            };
            F.HasShadow = true;
            F.VerticalOptions = LayoutOptions.Fill;
            StackLayout LL = new StackLayout {
                Children = {F},
                Padding = new Thickness(4,3),
                HeightRequest = 120,
                VerticalOptions = LayoutOptions.Fill
            };
            return new ViewCellExtended{
                View = LL
            };
        }


        public overview() : base(PanelSetEnum.psLeft)
        {
            Network N = new Network();
            List<Integreat.Location> locations = N.getAvailableLocations();
            List<Integreat.Language> languages = N.getAvailableLanguages(locations[0]);
            System.Collections.ObjectModel.ObservableCollection<Integreat.Page> pages = new System.Collections.ObjectModel.ObservableCollection<Page>(N.getPages(locations[0], languages[0]));


            var titleBar = new TitleBar(this, TitleBar.BarBtnEnum.bbLeftRightRight, TitleBar.BarAlignEnum.baTop);
            titleBar.BarColor = Color.FromHex("3f51b5");
            titleBar.Title = "Integreat " + locations[0].name;
            titleBar.TitleStyle = new Style(typeof(Label))
            {
                Setters = {
                    new Setter {Property = Label.TextColorProperty, Value = Color.White},
                    new Setter {Property = Label.XAlignProperty, Value = TextAlignment.Start },
                    new Setter {Property = Label.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };
            titleBar.BtnLeft.Source = ImageSource.FromFile("menu.png");
            titleBar.BtnLeft.Click += (sender, e) => IsShowLeftPanel = true;
            titleBar.BtnRightRight.Source = ImageSource.FromFile("search.png");

            SpeedAnimatePanel = 1000;


            RelativeLayout TopImage = new RelativeLayout {
                HeightRequest = 170
            };

            TopImage.Children.Add(new Image
            {
                Source = ImageSource.FromUri(new Uri(N.loadCityImage(locations[0]))),
                VerticalOptions = LayoutOptions.Start,
                Aspect = Aspect.AspectFill,
                HeightRequest = 170
            },
            Constraint.Constant(0),
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => { return parent.Width; }),
            heightConstraint: Constraint.Constant(180));
            
            TopImage.Children.Add(new Label 
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = " "+locations[0].name,
                BackgroundColor = Color.FromRgba(0,0,0,0.4),
                HeightRequest = 35,
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                
            },
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => { return parent.Height-30; }),
            Constraint.RelativeToParent((parent) => { return parent.Width; }),
            heightConstraint: Constraint.Constant(40));

             
            ListViewExtended CategoryList = new ListViewExtended();
            CategoryList.ItemTemplate = new DataTemplate(() => new CategoryListTemplate());
            CategoryList.ItemTapped += (object sender, ItemTappedEventArgs e) => ClosePanel(); 
            CategoryList.IsPullToRefreshEnabled = false;
            CategoryList.ItemsSource = pages;
            CategoryList.IsScrollable = true;
            CategoryList.VerticalOptions = LayoutOptions.Fill;
            CategoryList.InputTransparent = false;
            CategoryList.HasUnevenRows = false;
            CategoryList.BackgroundColor = Color.White;
            ListViewExtended SettingsList = new ListViewExtended
            {
                VerticalOptions = LayoutOptions.End,
                HeightRequest = 80,
                IsScrollable = false,
                SeparatorVisibility = SeparatorVisibility.None,
                InputTransparent = false
            };
            SettingsList.ItemTapped += delegate(object sender, ItemTappedEventArgs e)
            {
            };
            CategoryList.ItemTapped += delegate(object sender, ItemTappedEventArgs e)
            {
            };

           
            LeftPanel.BackgroundColor = Color.White;
            LeftPanel.Padding = new Thickness(0);
            LeftPanelWidth = (3*(App.ScreenBounds.Width/4));
            LeftPanel.InputTransparent = false;
            LeftPanel.AddToContext(
                new StackLayout
                {
                    
                    Padding = new Thickness(0, Device.OnPlatform(0, 0, 0), 0, 0),
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children =
                        {
                            TopImage,
                            CategoryList,
                            SettingsList
                        },HeightRequest = App.ScreenBounds.Height
                    },false);

            ListViewExtended ContentList = new ListViewExtended();
            ContentList.ItemTemplate = new DataTemplate(CreateCardViewTemplate);
            ContentList.ItemsSource = pages;
            ContentList.ItemTapped += (object sender, ItemTappedEventArgs e) => ClosePanel(); 
            ContentList.IsPullToRefreshEnabled = false;
            ContentList.IsScrollable = true;
            ContentList.VerticalOptions = LayoutOptions.Fill;
            ContentList.InputTransparent = false;
            ContentList.HasUnevenRows = false;
            ContentList.BackgroundColor = Color.White;
            ContentList.RowHeight = 120;
            ContentList.SeparatorVisibility = SeparatorVisibility.None;
            ContentLayout.Children.Add(titleBar);
            ContentLayout.Children.Add(ContentList);
        }
    }
}


