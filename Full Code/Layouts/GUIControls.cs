using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Folder_Locker.Layouts
{
    public static class GUIControls
    {
        public static class Welcome
        {
            public static void Set_FirstRun_Controls(Page page, Window window)
            {
                window.MinWidth = .75 * page.ActualWidth;
                window.MinHeight = .85 * page.ActualHeight;

                if (Globals.GetDimensions().X == -1)
                {
                    window.Width = window.MinWidth;
                }
                else
                {
                    window.Height = Globals.GetDimensions().Y;
                    window.Width = Globals.GetDimensions().X;
                }

                if (Globals.GetDimensions().X == -1) Globals.Set_Dimesions(window.Width, window.Height);
                window.Icon = ICONS.Folders.GetAppIcon();
            }

            public static void Set_GUI_Controls(Page page, List<object> controls)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is Button)
                    {
                        Button button = (Button)controls[i];
                        button.FontSize = page.ActualWidth / 60;
                        button.Width = page.ActualWidth / 10;
                        button.Height = page.ActualWidth / 30;
                    }
                    else if (controls[i] is Border)
                    {
                        Border bdr = (Border)controls[i];
                        bdr.Margin = new Thickness((page.ActualWidth / 55), 0, page.ActualWidth / 55, 0);
                    }
                    else if (controls[i] is Label)
                    {
                        Label lbl = (Label)controls[i];
                        if (lbl.Name == "companyName") lbl.FontSize = page.ActualWidth / 55;
                        else if (lbl.Name == "appName") lbl.FontSize = page.ActualWidth / 25;
                    }
                    else if (controls[i] is Image)
                    {
                        Image img = (Image)controls[i];
                        img.Width = img.Height = page.ActualWidth / 7.5;
                    }
                }
            }
        }

        public static class SignUp
        {
            public static void Set_FirstRun_Controls(Page page, Window window)
            {
                if(window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }

                window.Height = Globals.GetDimensions().Y;
                window.Width = Globals.GetDimensions().X;

                window.MinWidth = .75 * Globals.GetDimensions().X;
                window.MinHeight = Globals.GetDimensions().Y * 1.1;
                
                window.Icon = ICONS.Folders.GetAppIcon();
            }

            public static void Set_GUI_Controls(Page page, List<object> controls )
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is Button)
                    {
                        Button create_account = (Button)controls[i];
                        create_account.FontSize = page.ActualWidth / 55;
                    }
                    else if (controls[i] is Border)
                    {
                        Border bdr = (Border)controls[i];
                        bdr.Margin = new Thickness((page.ActualWidth / 55), 0, page.ActualWidth / 55, 0); 
                    }
                    else if (controls[i] is Label)
                    {
                        Label lbl = (Label)controls[i];
                        if (lbl.Name == "companyName") lbl.FontSize = page.ActualWidth / 55;
                        else if (lbl.Name == "heading") lbl.FontSize = page.ActualWidth / 25;
                    }
                    else if (controls[i] is ComboBox)
                    {
                        ComboBox gender = (ComboBox)controls[i];
                        gender.Width = page.ActualWidth/5;
                        gender.FontSize = page.ActualWidth / 55;
                    }
                    else if(controls[i] is Image)
                    {
                        Image companyLogo = (Image)controls[i];
                        companyLogo.Source = ICONS.Others.GetAppLogo();
                    }
                }
            }
        }

        public static class Login
        {
            public static void Set_GUI_Controls(Window page, List<object> controls)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is Button)
                    {
                        Button login = (Button)controls[i];
                        login.FontSize = page.ActualWidth / 18;
                        login.Height = page.ActualHeight / 13;
                        login.Width = page.Width * 0.8;
                        login.Margin = new Thickness(page.ActualWidth / 15, 0, page.ActualWidth / 15, page.ActualWidth / 20);
                    }
                    else if (controls[i] is Border)
                    {
                        Border bdr = (Border)controls[i];
                        bdr.Margin = new Thickness(page.ActualWidth / 15, 0, page.ActualWidth / 15, page.ActualWidth / 20);
                    }
                    else if (controls[i] is Label)
                    {
                        Label lbl = (Label)controls[i];
                        if (lbl.Name == "heading")
                            lbl.FontSize = page.ActualWidth / 10;

                        else if (!lbl.Name.Contains("validate"))
                            lbl.FontSize = page.ActualWidth / 30;

                        else if(lbl.Name.Contains("validate"))
                        {
                            lbl.FontSize = page.ActualWidth / 35;
                            lbl.Visibility = Visibility.Hidden;
                        }
                            
                    }
                    else if (controls[i] is TextBox)
                    {
                        TextBox username = (TextBox)controls[i];
                        username.FontSize = page.ActualWidth / 30;
                        username.Width = page.ActualWidth / 2.5;
                    }
                    else if (controls[i] is PasswordBox)
                    {
                        PasswordBox password = (PasswordBox)controls[i];
                        password.FontSize = page.ActualWidth / 30;
                        password.Width = page.ActualWidth / 2.5;
                    }
                }
            }
        }

        public static class SecureArea
        {
            public static void Set_FirstRun_Controls(Page page, Window window)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }

                window.Width = Globals.GetDimensions().X;
                window.Height = Globals.GetDimensions().Y;

                window.MinWidth = .75 * window.ActualWidth;
                window.MinHeight = window.ActualHeight;

                window.Icon = ICONS.Folders.GetAppIcon();
            }

            public static void Set_GUI_Controls(Page page, List<object> controls)
            {
                for(int i=0; i<controls.Count; i++)
                {
                    if(controls[i] is Button)
                    {
                        Button changeProfilePicBtn = (Button)controls[i];
                        changeProfilePicBtn.Width = page.ActualWidth / 6;
                        changeProfilePicBtn.Height = page.ActualWidth / 44;
                        changeProfilePicBtn.FontSize = page.ActualWidth / 65;
                    }
                    else if(controls[i] is Grid)
                    {
                        Grid subGrid = (Grid)controls[i];
                        subGrid.Margin = new Thickness(page.ActualWidth / 60, page.ActualWidth / 30, page.ActualWidth / 60, page.ActualWidth / 30);
                    }
                    else if(controls[i] is Ellipse)
                    {
                        Ellipse profilePicture = (Ellipse)controls[i];
                        profilePicture.Height = profilePicture.Width = page.ActualWidth / 7;
                    }
                    else if(controls[i] is Label)
                    {
                        Label operationStatus = (Label)controls[i];
                        operationStatus.FontSize = page.ActualWidth / 50;
                    }
                }
            }
        }
    }
}
