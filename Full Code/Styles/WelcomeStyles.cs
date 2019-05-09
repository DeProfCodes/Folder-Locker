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
    public static class WelcomeStyles
    {
        private static Style labels;

        public static void Set_StaticStyles(double width, Page welcome)
        {
            labels = new Style(typeof(Label))
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontFamilyProperty,
                            Value = new FontFamily("Yu Gothic UI Semibold")
                        },
                        new Setter
                        {
                            Property = Label.HorizontalAlignmentProperty,
                            Value = HorizontalAlignment.Center
                        },
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 60
                        }
                }
            };
            
            welcome.Resources["labels"] = labels;
        }

        private static void Labels(double width, Page welcome)
        {
            Style style = new Style(typeof(Label))
            {
                BasedOn = labels,
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 60
                        }
                }
            };

            welcome.Resources["labels"] = style;   
        }

        private static void BottomLabels(double width, Page welcome)
        {
            Style style = new Style(typeof(Label))
            {
                BasedOn = labels,
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 50
                        },
                        new Setter
                        {
                            Property = Label.ForegroundProperty,
                            Value = Brushes.Blue
                        }
                }
                
            };

            welcome.Resources["labelsBottom"] = style;
        }

        public static void Set_DynamicStyles(double width, Page welcome)
        {
            Labels(width, welcome);
            BottomLabels(width, welcome);
        }
    }
}
