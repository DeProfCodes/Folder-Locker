using Folder_Locker.Model;
using Folder_Locker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Folder_Locker.Layouts
{
    public static class ICONS
    {
        public static class Folders
        {
            public static string FolderIcon = "pack://application:,,,/Resources/Images/Folders/Enabled/folder_lock icon2.PNG";

            public static string NewFolder = "pack://application:,,,/Resources/Images/Folders/Enabled/folder-add-icon.PNG"; 

            public static string NewFolderDisabled = "pack://application:,,,/Resources/Images/Folders/Disabled/folder-add-iconD.PNG";

            public static string RemoveFolder = "pack://application:,,,/Resources/Images/Folders/Enabled/folder-remove-icon.ico";

            public static string RemoveFolderDisabled = "pack://application:,,,/Resources/Images/Folders/Disabled/folder-remove-iconD.png";

            public static BitmapImage GetAppIcon()
            {
                return new BitmapImage(new Uri(FolderIcon));
            }
        }

        public static class Locks
        {
            public static string Lock = "pack://application:,,,/Resources/Images/Locks/lock.png";

            public static string LockDisabled = "pack://application:,,,/Resources/Images/Locks/lock D.png";

            public static string Unlock = "pack://application:,,,/Resources/Images/Locks/lock_open.png";

            public static string UnlockDisabled = "pack://application:,,,/Resources/Images/Locks/lock_open D.png";
            
            private static string LockClosedIcon = "../Resources/Images/Locks/locked-icon.png";

            private static string LockOpenIcon = "../Resources/Images/Locks/unlocked-icon.png";

            public static string GetLockOpenIconSource()
            {
                return LockOpenIcon;
            }

            public static string GetLockClosedIconSource()
            {
                return LockClosedIcon;
            }
        }

        private static class Account
        {
            public static string ProfilePictureMale = "pack://application:,,,/Resources/Images/Account/profile-male.jpg";

            public static string ManageAccounts = "pack://application:,,,/Resources/Images/Account/ManageAcc ico2.png";

            public static string RemoveAccount = "pack://application:,,,/Resources/Images/Account/remove-icon.png";

            public static string ProfileIcon = "pack://application:,,,/Resources/Images/Profile-icon.PNG";
        }
        
        public static class Others
        {
            private static string exitIcon = "pack://application:,,,/Resources/Images/Other/exit icon.png";
            private static string appLogo = "pack://application:,,,/Resources/Images/Other/Logo1.PNG";
            private static string settingsIcon = "pack://application:,,,/Resources/Images/Other/settings icon.png";
            private static string homeIcon = "pack://application:,,,/Resources/Images/Other/home.png";

            public static ImageSource GetExitIcon()
            {
                return new BitmapImage(new Uri(exitIcon));
            }

            public static ImageSource GetAppLogo()
            {
                return new BitmapImage(new Uri(appLogo));
            }

            public static ImageSource GetsettingsIcon()
            {
                return new BitmapImage(new Uri(settingsIcon));
            }

            public static ImageSource GetHomeIcon()
            {
                return new BitmapImage(new Uri(homeIcon));
            }
        }

        public static void SetFolderIcons(Page page)
        {

            BitmapImage newFolder = new BitmapImage(new Uri(Folders.NewFolder));
            BitmapImage newFolderD = new BitmapImage(new Uri(Folders.NewFolderDisabled));
            BitmapImage remFolder = new BitmapImage(new Uri(Folders.RemoveFolder));
            BitmapImage remFolderD = new BitmapImage(new Uri(Folders.RemoveFolderDisabled));

            page.Resources.Add("newFolder", newFolder);
            page.Resources.Add("newFolderD", newFolderD);
            page.Resources.Add("removeFolder", remFolder);
            page.Resources.Add("removeFolderDisabled", remFolderD);
        }

        public static void SetLocksIcons(Page page)
        {
            BitmapImage locked = new BitmapImage(new Uri(Locks.Lock));
            BitmapImage lockedD = new BitmapImage(new Uri(Locks.LockDisabled));
            BitmapImage unlocked = new BitmapImage(new Uri(Locks.Unlock));
            BitmapImage unlockedD = new BitmapImage(new Uri(Locks.UnlockDisabled));

            page.Resources.Add("lock_icon", locked);
            page.Resources.Add("lockDisabled", lockedD);
            page.Resources.Add("unlock_icon", unlocked);
            page.Resources.Add("unlockDisabled", unlockedD);
        }

        public static void SetAccountIcons(Page page)
        {
            BitmapImage profilePicM = new BitmapImage(new Uri(Account.ProfilePictureMale));
            BitmapImage manageAcc = new BitmapImage(new Uri(Account.ManageAccounts));
            BitmapImage removeAcc = new BitmapImage(new Uri(Account.RemoveAccount));
            BitmapImage profileIcon = new BitmapImage(new Uri(Account.ProfilePictureMale));

            page.Resources.Add("ProfilePicMale", profilePicM);
            page.Resources.Add("ManageAccount", manageAcc);
            page.Resources.Add("RemoveAccount", removeAcc);
            page.Resources.Add("ProfileIcon", profileIcon);
        }

        public static class SecureArea
        {
            public static void GetAccountIcons(Page page, List<Image> icons, Ellipse ProfilePic)
            {
                for (int i = 0; i < icons.Count; i++)
                {
                    switch (icons[i].Name)
                    {
                        case "manageAcc_icon": icons[i].Source = (ImageSource)page.Resources["ManageAccount"]; break;
                        case "removeAcc_icon": icons[i].Source = (ImageSource)page.Resources["RemoveAccount"]; break;
                        case "accSettings_icon": icons[i].Source = Others.GetsettingsIcon(); break;
                        case "exit_icon": icons[i].Source = Others.GetExitIcon(); break;
                        case "home_icon": icons[i].Source = Others.GetHomeIcon(); break;
                    }
                }

                ImageBrush profileP = new ImageBrush();
                profileP.ImageSource = (ImageSource)page.Resources["ProfilePicMale"];
                ProfilePic.Fill = profileP;
            }

            public static void Set_StartUpIcons(Page page, List<Image> icons, Ellipse ProfilePic)
            {
                SetFolderIcons(page);
                SetLocksIcons(page);
                SetAccountIcons(page);

                for (int i = 0; i < icons.Count; i++)
                {
                    switch (icons[i].Name)
                    {
                        case "add_newFolder_ico": icons[i].Source = (ImageSource)page.Resources["newFolder"]; break;
                        case "f_remove_ico": icons[i].Source = (ImageSource)page.Resources["removeFolderDisabled"]; break;
                        case "f_lock_ico": icons[i].Source = (ImageSource)page.Resources["lockDisabled"]; break;
                        case "f_unlock_ico": icons[i].Source = (ImageSource)page.Resources["unlockDisabled"]; break;
                        default: GetAccountIcons(page, icons, ProfilePic); break;
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

            public static void Reset_FolderIcons(Page page, List<Image> icons, List<Border> borders)
            {
                Image f_lock_ico = icons.FirstOrDefault(x => x.Name == "f_lock_ico");
                Image f_unlock_ico = icons.FirstOrDefault(x => x.Name == "f_unlock_ico");
                Image f_remove_ico = icons.FirstOrDefault(x => x.Name == "f_remove_ico");

                Border unlock_folder = borders.FirstOrDefault(x => x.Name == "unlock_folder");
                Border lock_folder = borders.FirstOrDefault(x => x.Name == "lock_folder");
                Border remove_folder = borders.FirstOrDefault(x => x.Name == "remove_folder");

                f_lock_ico.Source = (ImageSource)page.Resources["lockDisabled"];
                lock_folder.IsEnabled = false;

                f_remove_ico.Source = (ImageSource)page.Resources["removeFolderDisabled"];
                remove_folder.IsEnabled = false;

                f_unlock_ico.Source = (ImageSource)page.Resources["unlockDisabled"];
                unlock_folder.IsEnabled = false;

                FolderView.UnselectRow();
            }
        }

        public static class AccountSettings
        {
            public static void Set_StartUpIcons(Page page, List<Image> icons, Ellipse ProfilePic)
            {
                SetAccountIcons(page);

                SecureArea.GetAccountIcons(page, icons, ProfilePic);
            }
        }
    }
}

