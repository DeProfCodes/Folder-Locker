using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Folder_Locker.Resources;

namespace Folder_Locker.Views
{
    public static class GUIControls
    {
        public static class SecureArea
        {
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

            public static void Set_FirstRun_Controls(Page page, Window window)
            {
                window.MinWidth = .75 * page.ActualWidth;
                window.MinHeight = window.ActualHeight;

                window.MaxHeight = window.ActualHeight;
                window.MaxWidth = window.ActualWidth;

                window.Icon = ICONS.Folders.GetAppIcon();
            }
        }
        
    }
}
