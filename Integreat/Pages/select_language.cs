using System;
using Xamarin.Forms;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;

namespace Integreat
{

    public class select_language : SideBarPage
    {
        System.Collections.ObjectModel.ObservableCollection<language_item> languages = new System.Collections.ObjectModel.ObservableCollection<language_item>() ;

        public select_language() : base(PanelSetEnum.psLeftRight)
        {
            var titleBar = new TitleBar(this, TitleBar.BarBtnEnum.bbLeftRight);
            titleBar.BarColor = Color.FromRgb(253,227,0);
            titleBar.Title = "SELECT A LANGUAGE\r\nWhat language do you speak?";
            titleBar.TitleStyle = new Style(typeof(Label))
            {
                    Setters = {
                        new Setter {Property = Label.TextColorProperty, Value = Color.White},
                        new Setter {Property = Label.XAlignProperty, Value = TextAlignment.Center },
                        new Setter {Property = Label.FontAttributesProperty, Value = FontAttributes.Bold }
                    }
            };

            languages.Add(new language_item { Title = "English",  ImageFilename = "https://upload.wikimedia.org/wikipedia/en/thumb/a/ae/Flag_of_the_United_Kingdom.svg/100px-Flag_of_the_United_Kingdom.svg.png" });
            languages.Add(new language_item { Title = "Français", ImageFilename = "https://upload.wikimedia.org/wikipedia/en/thumb/c/c3/Flag_of_France.svg/100px-Flag_of_France.svg.png" });
            languages.Add(new language_item { Title = "Deutsch",  ImageFilename = "https://upload.wikimedia.org/wikipedia/en/thumb/b/ba/Flag_of_Germany.svg/100px-Flag_of_Germany.svg.png" });


            var layout = new RelativeLayout();

            StackLayout box1; 

            double padding = 10;


            StackLayout last = null;
            foreach(var lang in languages) 
            {
                var relativeTo = last; // local copy
                var box = new StackLayout
                    {
                        BackgroundColor = Color.White,
                        Padding = new Thickness(10),
                        Children = {
                            new Image {
                                HeightRequest = 70,
                                WidthRequest = 70,
                                Source = ImageSource.FromUri(new Uri(lang.ImageFilename)),
                                Aspect = Aspect.AspectFit
                            },
                            new Label
                            {
                                XAlign = TextAlignment.Center,
                                Text = lang.Title,
                                TextColor = Color.Black
                            }
                        }
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

        public class language_item {
            public language_item() {}

            public string Title { get; set; }
            public string ImageFilename { get; set; }
        }

        public class language_button : ViewCell {

            public language_button() {
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


