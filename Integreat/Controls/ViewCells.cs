using System;
using Xamarin.Forms;

namespace Integreat
{
    public class ViewCellExtended : ViewCell
    {
        #region IsHighlightSelection
        public static readonly BindableProperty IsHighlightSelectionProperty =
            BindableProperty.Create<ViewCellExtended, bool>(p => p.IsHighlightSelection, true);

        public bool IsHighlightSelection
        {
            get { return (bool)GetValue(IsHighlightSelectionProperty); }
            set { SetValue(IsHighlightSelectionProperty, value); }
        }
        #endregion
        #region SelectColor
        public static readonly BindableProperty SelectColorProperty =
            BindableProperty.Create<ViewCellExtended, Color>(p => p.SelectColor, Color.Transparent);

        public Color SelectColor
        {
            get { return (Color)GetValue(SelectColorProperty); }
            set { SetValue(SelectColorProperty, value); }
        }
        #endregion
    }

    public class ValueConverter : IValueConverter
    {
        public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return new FileImageSource {
                    File = value.ToString ()
                };

            }
            catch (Exception ex)
            {
                return null;   
            }
        }

        public object ConvertBack (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException ();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        }
    }

    class CategoryListTemplate : ViewCellExtended
    {
        public CategoryListTemplate()
        {
            SelectColor = Color.FromRgb(255,0,0);

            Grid gridMenuItem = new Grid
                {
                    BackgroundColor = Color.Transparent,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(10, 0),
                    RowDefinitions = 
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                        },
                    ColumnDefinitions = 
                        {
                            new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star)},
                            new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star)}
                        },

                };

            var imgMenuItem = new Image();

            imgMenuItem.SetBinding(Image.SourceProperty,"thumbnail", BindingMode.OneWay, new ValueConverter());
            gridMenuItem.Children.Add(imgMenuItem, 0, 0);

            var txtMenuItem = new Label
                {
                    TextColor = Color.FromRgb(0,0,0),
                };
            txtMenuItem.SetBinding(Label.TextProperty, new Binding("title", stringFormat: "{0}"));
           // txtMenuItem.SetBinding(Label.TextProperty, "title");
            gridMenuItem.Children.Add(txtMenuItem, 1, 0);

            View = gridMenuItem;
        }
    }
 
}