using Folder_Locker.Database;
using Folder_Locker.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Folder_Locker.Services
{
    public static class Navigation
    {
        public static void GotoWelcome(Page page)
        {
           page.NavigationService.Navigate(new Uri("Pages/Welcome.xaml", UriKind.Relative));
        }
        
        public static void GotoSignUp(Page page)
        {
            page.NavigationService.Navigate(new Uri("Pages/SignUp.xaml", UriKind.Relative));
        }

        public static void GotoSecureArea(Page page)
        {
            page.NavigationService.Navigate(new Uri("Pages/SecureArea.xaml", UriKind.Relative));
        }

        public static void GotoManageAccounts(Page page)
        {
            page.NavigationService.Navigate(new Uri("Pages/ManageAccounts.xaml", UriKind.Relative));
        }

        public static void GotoAccountSettings(Page page)
        {
            page.NavigationService.Navigate(new Uri("Pages/AccountSettings.xaml", UriKind.Relative));
        }

        public static void GotoEditAccount(Page page)
        {
            page.NavigationService.Navigate(new Uri("Pages/EditAccount.xaml", UriKind.Relative));
        }

        public static class MenuItems
        {
            private static void OpenDocument(string document)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    string path = AppDomain.CurrentDomain.BaseDirectory + $@"{document}.pdf";
                    Uri pdf = new Uri(path, UriKind.RelativeOrAbsolute);
                    process.StartInfo.FileName = pdf.LocalPath;
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            public static void Navigate(Page page, string menuItem, Window window)
            {
                switch (menuItem)
                {
                    case "login_lbl":
                        LogIn logIn = new LogIn(window, page);
                        logIn.Owner = window;
                        window.Hide();
                        logIn.Show();
                        break;
                    case "logout_lbl":
                        AppServices.LogOut.Exit(page);
                        break;

                    case "navigateBack_lbl":
                        GotoWelcome(page);
                        break;

                    case "exit_lbl":
                        AppServices.CloseApplication(window);
                       break;

                    case "howToUse_lbl":
                        OpenDocument("HowToUse");
                        break;

                    case "about_lbl":
                        OpenDocument("AboutSoftware");
                        break;
                }
            }
        }

        public static class MainNavigation
        {
            public static void Navigate(Page page, string sender, Window window)
            {
                switch (sender)
                {
                    case "home":
                        GotoSecureArea(page);
                        break;

                    case "exit":
                        AppServices.CloseApplication(window);
                        break;

                    case "manage_accs":
                        if (Globals.GetActiveUsername() == DatabaseSQL.GetFirstUser())
                        {
                            GotoManageAccounts(page);
                        }
                        else
                        {
                            MessageBox.Show("Sorry, you don't have previledges to access this feature!", "Access Denied!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case "deleteAccount":
                        AppServices.RemoveAccount.DeleteAccount(page, Globals.GetSelectedAccount().Username);
                        break;

                    case "addAccount":
                        GotoSignUp(page);
                        break;

                    case "editAccount":
                        GotoEditAccount(page);
                        break;

                    case "acc_settings":
                        GotoAccountSettings(page);
                        break;

                    case "remove_acc":
                        AppServices.RemoveAccount.DeleteAccount(page, Globals.GetActiveUsername());
                        break;
                }
            }
        }
    }
}
