using System;
using Xamarin.Forms;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;

namespace Integreat
{

    public class select_location : SideBarPage
    {
        System.Collections.ObjectModel.ObservableCollection<location_item> locations = new System.Collections.ObjectModel.ObservableCollection<location_item>() ;

        public select_location() : base(PanelSetEnum.psLeftRight)
        {


            //[{"id":"2","name":"Augsburg","icon":"http:\/\/vmkrcmar21.informatik.tu-muenchen.de\/wordpress\/wp-content\/uploads\/sites\/2\/2015\/10\/cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg","path":"\/wordpress\/augsburg\/","description":"Augsburg","global":false},{"id":"8","name":"D\u00fcsseldorf","icon":"http:\/\/vmkrcmar21.informatik.tu-muenchen.de\/wordpress\/wp-content\/uploads\/sites\/8\/2015\/11\/cropped-Wappen_der_Landeshauptstadt_Duesseldorf1.png","path":"\/wordpress\/duesseldorf\/","description":"D\u00fcsseldorf","global":false},{"id":"15","name":"Main-Taunus-Kreis","icon":"http:\/\/vmkrcmar21.informatik.tu-muenchen.de\/wordpress\/wp-content\/uploads\/sites\/15\/2015\/11\/cropped-main-taunus-kreis-logo-twitter.png","path":"\/wordpress\/main-taunus-kreis\/","description":"Main-Taunus-Kreis","global":false}]

            var titleBar = new TitleBar(this, TitleBar.BarBtnEnum.bbLeftRight);
            titleBar.BarColor = Color.FromRgb(253,227,0);
            titleBar.Title = "SELECT A LOCATION\r\nWhere are you?";
            titleBar.TitleStyle = new Style(typeof(Label))
                {
                    Setters = {
                        new Setter {Property = Label.TextColorProperty, Value = Color.White},
                        new Setter {Property = Label.XAlignProperty, Value = TextAlignment.Center },
                        new Setter {Property = Label.FontAttributesProperty, Value = FontAttributes.Bold }
                    }
                };

            locations.Add(new location_item { Name = "Augsburg",   Color = Color.Blue, ImageFilename = "http://www.ina-sic.de/bilder/default/augsburg2.png" });
            locations.Add(new location_item { Name = "Berlin",     Color = Color.Blue, ImageFilename = "http://www.briefmarkenverein-berliner-baer.de/foto-berliner-baer/berliner-baer-wappen.gif" });
            locations.Add(new location_item { Name = "Düsseldorf", Color = Color.Blue, ImageFilename = "http://www.designtagebuch.de/wp-content/uploads/mediathek//duesseldorf-marke-logo.png" });
            locations.Add(new location_item { Name = "Freiburg",   Color = Color.Blue, ImageFilename = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/81/Wappen_Freiburg_im_Breisgau.svg/140px-Wappen_Freiburg_im_Breisgau.svg.png\"" });
            locations.Add(new location_item { Name = "München",    Color = Color.Blue, ImageFilename = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/17/Muenchen_Kleines_Stadtwappen.svg/818px-Muenchen_Kleines_Stadtwappen.svg.png" });


            var layout = new RelativeLayout();
            layout.BackgroundColor = Color.FromHex("EEEEEE");

            XLabs.Forms.Controls.ImageButton box1; 

            double padding = Device.OnPlatform<int>(22,10,10);
            System.IO.MemoryStream M = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("augsburg2.png"));

            XLabs.Forms.Controls.ImageButton last = null;
            foreach(var lang in locations) 
            {
                var relativeTo = last; // local copy

                var box = new XLabs.Forms.Controls.ImageButton
                    {
                        ImageWidthRequest = ((int)(App.ContentBounds.Width/2)-90),
                        ImageHeightRequest = ((int)(App.ContentBounds.Width/2)-90),

                        Source = new FileImageSource {File ="augsburg2.png"},
                        Text = lang.Name,
                        BorderColor = Color.FromHex("C9C9C9"),
                        BorderWidth = 2,
                        Orientation = XLabs.Enums.ImageOrientation.ImageOnTop,
                        BorderRadius = 10,
                        TextColor = Color.Black,
                        BackgroundColor = Color.FromHex("FAFAFA")
                    };
                if (last != null)
                {
                    Func<View, bool> pastBounds = view => relativeTo.Bounds.Right + padding + view.Width > layout.Width;
                    layout.Children.Add(box, () => new Rectangle(pastBounds(relativeTo) ? box1.X : relativeTo.Bounds.Right + padding,
                        pastBounds(relativeTo) ? relativeTo.Bounds.Bottom + padding : relativeTo.Y,
                        relativeTo.Width,
                        relativeTo.Height));
                } else {
                    box1 = box;
                    layout.Children.Add( box1, () => new Rectangle(((layout.Width + padding) % 40) / 2, padding, (App.ContentBounds.Width/2)-30,(App.ContentBounds.Width/2)-30));
                }
                last = box;
            }




            Content = new StackLayout
                { 
                    BackgroundColor = Color.FromHex("EEEEEE"),
                    Padding = new Thickness(0),
                    Children =
                        {
                            titleBar,
                            new ScrollView { Content = layout, Padding = new Thickness(6) }

                        }
                    };
            /*var parser = new Parser();
            var stylesheet = parser.Parse(".selectLanguageTitle { background-color:purple; text-align:center; }");

            foreach (ExCSS.StyleRule rule in stylesheet.Rules)
            {
                foreach (var declaration in rule.Declarations)
                {
                    foreach (var controlItem in ((StackLayout)this.Content).Children)
                    {
                        if (controlItem.ClassId == rule.Value.Substring(1))
                        {
                            var XStyle = new Style(controlItem.GetType());
                            if (declaration.Name == "background-color")
                            {
                                var Z = declaration.Term.ToString().ToColor();
                                XStyle.Setters.Add(new Setter
                                    { 
                                        Property = VisualElement.BackgroundColorProperty,
                                        Value = Z
                                    });
                            }
                            controlItem.Style = XStyle;
                        }
                    }                    
                }
            }*/

        }

        public class location_item {
            public location_item() {}

            public string Name { get; set; }
            public Uri Url { get; set; }
            public Color Color { get; set; }
            public string ImageFilename { get; set; }
        }

        public class location_button : ViewCell {

            public location_button() {
                Label TitleLabel = new Label();
                TitleLabel.SetBinding (
                    Label.TextProperty, new Binding (
                        "Title", 
                        stringFormat: "{0}")
                );
                this.View = new StackLayout {

                    Children = {
                        TitleLabel
                    }
                };
            }
        }


    }
}


