using Folder_Locker.Model;
using Folder_Locker.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Folder_Locker.Views
{
    public static class GUIElements
    {
        public static class SecureArea
        {
            public static void GetAccountIcons(Page page, List<Image> icons, Ellipse ProfilePic)
            {
                for (int i = 0; i < icons.Count; i++)
                {
                    switch (icons[i].Name)
                    {
                        case "manageAcc_icon"    : icons[i].Source = (ImageSource)page.Resources["ManageAccount"]; break;
                        case "removeAcc_icon"   : icons[i].Source = (ImageSource)page.Resources["RemoveAccount"]; break;
                        case "accSettings_icon" : icons[i].Source = ICONS.Others.GetsettingsIcon(); break;
                        case "exit_icon"        : icons[i].Source = ICONS.Others.GetExitIcon(); break;
                    }
                }

                ImageBrush profileP = new ImageBrush();
                profileP.ImageSource = (ImageSource)page.Resources["ProfilePicMale"];
                ProfilePic.Fill = profileP;
            }

            public static void Set_StartUpIcons(Page page, List<Image> icons, Ellipse ProfilePic)
            {
                ICONS.SetFolderIcons(page);
                ICONS.SetLocksIcons(page);
                ICONS.SetAccountIcons(page);
                
                for (int i = 0; i < icons.Count; i++)
                {
                    switch (icons[i].Name)
                    {
                        case "add_newFolder_ico": icons[i].Source = (ImageSource)page.Resources["newFolder"]; break;
                        case "f_remove_ico"     : icons[i].Source = (ImageSource)page.Resources["removeFolderDisabled"]; break;
                        case "f_lock_ico"       : icons[i].Source = (ImageSource)page.Resources["lockDisabled"]; break;
                        case "f_unlock_ico"     : icons[i].Source = (ImageSource)page.Resources["unlockDisabled"]; break;
                             default : GetAccountIcons(page, icons, ProfilePic); break;
                    }
                }
            }

            public static void Toggle_FolderIcons(Page page, List<Image> icons, List<Border> borders)
            {
                Image f_lock_ico = icons.FirstOrDefault(x => x.Name == "f_lock_ico");
                Image f_unlock_ico = icons.FirstOrDefault(x => x.Name == "f_unlock_ico");
                Image f_remove_ico = icons.FirstOrDefault(x => x.Name == "f_remove_ico");

                Border unlock_folder = borders.FirstOrDefault(x => x.Name == "unlock_folder");
                Border lock_folder = borders.FirstOrDefault(x => x.Name == "lock_folder");
                Border remove_folder = borders.FirstOrDefault(x => x.Name == "remove_folder");

                Folder currentFolder = Globals.GetCurrentFolder();

                if (currentFolder.FolderStatus == "LOCKED")
                {
                    f_unlock_ico.Source = (ImageSource)page.Resources["unlock_icon"];
                    unlock_folder.IsEnabled = true;

                    f_lock_ico.Source = (ImageSource)page.Resources["lockDisabled"];
                    lock_folder.IsEnabled = false;

                    f_remove_ico.Source = (ImageSource)page.Resources["removeFolderDisabled"];
                    remove_folder.IsEnabled = false;
                }
                else
                {
                    f_lock_ico.Source = (ImageSource)page.Resources["lock_icon"];
                    lock_folder.IsEnabled = true;

                    f_unlock_ico.Source = (ImageSource)page.Resources["unlockDisabled"];
                    unlock_folder.IsEnabled = false;

                    f_remove_ico.Source = (ImageSource)page.Resources["removeFolder"];
                    remove_folder.IsEnabled = true;
                }

            }
        }

        public static class AccountSettings
        {
            public static void Set_StartUpIcons(Page page, List<Image> icons, Ellipse ProfilePic)
            {
                ICONS.SetAccountIcons(page);

                SecureArea.GetAccountIcons(page, icons, ProfilePic);
            }
        }
    }
}

