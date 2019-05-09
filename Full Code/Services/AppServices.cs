using Folder_Locker.Database;
using Folder_Locker.Model;
using Folder_Locker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Folder_Locker.Services
{
    public static class AppServices
    {
        public static void CloseApplication(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the application?", "EXIT?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) window.Close();
        }
        
        public static void ChangeProfilePicture()
        {
            MessageBox.Show("Coming soon!");
            #region TO EXPLORE
            /* OpenFileDialog op = new OpenFileDialog();
           op.Title = "Select a picture";
           op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
             "Portable Network Graphic (*.png)|*.png";


           ImageBrush imgBrush = new ImageBrush();
           Uri uri = null;

           if (op.ShowDialog() == true)
           {
               uri = new Uri(op.FileName);
               imgBrush.ImageSource = new BitmapImage(uri);
               profile_ico.Fill = imgBrush;

           }

           string fileSaveDirectory = "../../Resources/Images/ProfilePic.png";
           using (FileStream stream = new FileStream(fileSaveDirectory, FileMode.Create))
           {
               PngBitmapEncoder encoder = new PngBitmapEncoder();
               encoder.Frames.Add(BitmapFrame.Create(uri));
               encoder.Save(stream);

               //encoder.
               //var p = new Microsoft.Build.Evaluation.Project(@"C:\projects\BabDb\test\test.csproj");
               //p.AddItem("Folder", @"C:\projects\BabDb\test\test2");
               //p.AddItem("Compile", @"C:\projects\BabDb\test\test2\Class1.cs");
               //p.Save();
           }

           // Tell the user we're done. 
           */
            #endregion
        }

        public static class LogOut
        {
            private static void logOut(Page page)
            {
                page.NavigationService.Navigate(new Uri("Pages/Welcome.xaml", UriKind.Relative));
                Globals.Set_LoggedIn(false);
            }

            public static void Exit(Page page)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to Logout?", "LOGOUT", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        logOut(page);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
        
        public static class RemoveAccount
        {
            private static void removeAccount(Page page, string username)
            {
                DatabaseSQL.RemoveUserRecord(username);
                AccountViewModel.LoadAccountsFromDB();

                if (username == Globals.GetActiveUsername())
                {
                    Navigation.GotoWelcome(page);
                    MessageBox.Show("Your account has been deleted from the database, Good bye","Account deleted",MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {// Account deleted in the manage accounts page, no navigation required
                    MessageBox.Show("Account has been successfully deleted!", "Accout deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            public static void DeleteAccount(Page page, string username)
            {
                MessageBoxResult outcome = MessageBox.Show("Are you sure you want to delete your account, including all information associated with it?", "Delete Account?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (outcome == MessageBoxResult.Yes)
                {
                    if (username == DatabaseSQL.GetFirstUser())
                    {
                        MessageBoxResult lastConfirm = MessageBox.Show($"If you proceed to delete this account, you are giving up total contol to another account '{DatabaseSQL.GetSecondUser()}', procced?", "Delete Account", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (lastConfirm == MessageBoxResult.Yes)
                        {
                            removeAccount(page, username);
                            Globals.Set_LoggedIn(false);
                        }
                        return;
                    }
                    removeAccount(page, username);
                }
            }
            
        }
    }
}
