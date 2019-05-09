using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Folder_Locker.Styles
{
    public static class SecureAreaStyles
    {
        
        public static void FrameBorders(double width, Page secureArea)
        {
            Style style = new Style(typeof(Border))
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = Border.CornerRadiusProperty,
                            Value = new CornerRadius(15)
                        },
                        new Setter
                        {
                            Property = Border.WidthProperty,
                            Value = width / 15
                        },
                        new Setter
                        {
                            Property = Border.HeightProperty,
                            Value = width / 15
                        }
                },
                Triggers =
                {
                    new Trigger
                    {
                        Property = Border.IsMouseOverProperty,
                        Value = true,
                        Setters =
                        {
                            new Setter
                            {
                                Property = Border.BackgroundProperty,
                                Value = Brushes.LightBlue
                            }
                        }
                    }

                }
            };

            secureArea.Resources["borders"] = style;
        }

        public static void ImageIcons(double width, Page secureArea)
        {
            Style style = new Style(typeof(Image))
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = Image.HorizontalAlignmentProperty,
                            Value = HorizontalAlignment.Center
                        },
                        new Setter
                        {
                            Property = Image.WidthProperty,
                            Value = width / 20
                        },
                        new Setter
                        {
                            Property = Image.HeightProperty,
                            Value = width / 20
                        }
                }
            };

            secureArea.Resources["ImageContainer"] = style;
        }

        public static void SetStyles(double width, Page secureArea)
        {
            FrameBorders(width, secureArea);
            ImageIcons(width, secureArea);
        }

        public static void FolderPropertiesBorders(double width, UserControl folderView)
        {
            Style style = new Style(typeof(Border))
            {
                Triggers =
                {
                    new Trigger
                    {
                        Property = Border.IsMouseOverProperty,
                        Value = true,
                        Setters =
                        {
                            new Setter
                            {
                                Property = Border.BackgroundProperty,
                                Value = Brushes.Gold
                            }
                        }
                    }

                }
            };

            folderView.Resources["borders"] = style;

        }
    }
}
