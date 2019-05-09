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
    public static class SignUpStyles
    {
        private static Style entryCells, passwdCells, cell_lbl, validate_lbl;
        
        public static void Set_StaticStyles(double width, Page signUp)
        {
            entryCells = new Style
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = TextBox.VerticalAlignmentProperty,
                            Value = VerticalAlignment.Center
                        },
                        new Setter
                        {
                            Property = TextBox.HorizontalAlignmentProperty,
                            Value = HorizontalAlignment.Left
                        },
                        new Setter
                        {
                            Property = TextBox.FontSizeProperty,
                            Value = width / 55
                        },
                        new Setter
                        {
                            Property = TextBox.WidthProperty,
                            Value = width / 5
                        }
                }
            };

            passwdCells = new Style
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = PasswordBox.VerticalAlignmentProperty,
                            Value = VerticalAlignment.Center
                        },
                        new Setter
                        {
                            Property = PasswordBox.HorizontalAlignmentProperty,
                            Value = HorizontalAlignment.Left
                        },
                        new Setter
                        {
                            Property = PasswordBox.FontSizeProperty,
                            Value = width / 55
                        },
                        new Setter
                        {
                            Property = PasswordBox.WidthProperty,
                            Value = width / 5
                        }
                }
            };

            cell_lbl = new Style(typeof(Label))
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.VerticalAlignmentProperty,
                            Value = VerticalAlignment.Center
                        },
                        new Setter
                        {
                            Property = Label.HorizontalAlignmentProperty,
                            Value = HorizontalAlignment.Right
                        },
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 55
                        },
                        new Setter
                        {
                            Property = Label.ForegroundProperty,
                            Value = Brushes.DarkGreen
                        }
                }
            };

            validate_lbl = new Style(typeof(Label))
            {
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 65
                        },
                        new Setter
                        {
                            Property = Label.ForegroundProperty,
                            Value = Brushes.Red
                        }
                }
            };
            
            signUp.Resources["EntryBoxes"] = entryCells;
            signUp.Resources["PasswdBox"] = passwdCells;
            signUp.Resources["labels"] = cell_lbl;
            signUp.Resources["validationLabels"] = validate_lbl;
        }

        public static void EntryCells(double width, Page signUp)
        {
            Style style = new Style(typeof(TextBox))
            {
                BasedOn = entryCells,
               
                Setters =
                    {
                        new Setter
                        {
                            Property = TextBox.FontSizeProperty,
                            Value = width / 55
                        },
                        new Setter
                        {
                            Property = TextBox.WidthProperty,
                            Value = width / 5
                        }
                }
            };

            signUp.Resources["EntryBoxes"] = style;
        }

        private static void PasswordCells(double width, Page signUp)
        {
            Style style = new Style(typeof(PasswordBox))
            {
                BasedOn = passwdCells,
                Setters =
                    {
                        new Setter
                        {
                            Property = PasswordBox.FontSizeProperty,
                            Value = width / 55
                        },
                        new Setter
                        {
                            Property = PasswordBox.WidthProperty,
                            Value = width / 5
                        }
                }
            };

            signUp.Resources["PasswdBox"] = style;
        }

        private static void LabelsForCells(double width, Page signUp)
        {
            Style style = new Style(typeof(Label))
            {
                BasedOn = cell_lbl,
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 55
                        }
                }
            };

            signUp.Resources["labels"] = style;
        }

        public static void LabelZForCells(double width, Page signUp)
        {
            Style style = new Style(typeof(Label))
            {
                BasedOn = cell_lbl,
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 55
                        }
                }
            };

            signUp.Resources["labelZ"] = style;
        }

        private static void LabelsForValidations(double width, Page signUp)
        {
            Style style = new Style(typeof(Label))
            {
                BasedOn = validate_lbl,
                Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = width / 65
                        }
                }
            };

            signUp.Resources["validationLabels"] = style;
        }

        public static void SetDynamicStyles(double width, Page signUp)
        {
            EntryCells(width, signUp);
            PasswordCells(width, signUp);
            LabelsForCells(width, signUp);
            LabelsForValidations(width, signUp);
        }

    }
}
