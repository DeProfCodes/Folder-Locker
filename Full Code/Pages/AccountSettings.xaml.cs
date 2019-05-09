// User defined namespaces 
using Folder_Locker.Database;
using Folder_Locker.Model;
using Folder_Locker.Services;
// System Libraries
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Folder_Locker.Layouts;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings : Page
    {
        private Window window;
        private bool firstRun;
        private Enum changeButton;
        private enum Changes {Email, Cellphone, Password};
        private List<Image> pageIcons;
        private List<object> otherControls;

        public AccountSettings()
        {
            InitializeComponent();

            StartUps();
        }

        private void StartUps()
        {
            firstRun = true;

            pageIcons = new List<Image>
            {
                accSettings_icon, manageAcc_icon,
                removeAcc_icon, exit_icon
            };

            otherControls = new List<object> { subGrid1, profile_ico, changeProfilePic };

            ICONS.AccountSettings.Set_StartUpIcons(this, pageIcons, profile_ico);
            ShowsNavigationUI = true;

            // Hide some controls to appear on change request
            changeRequestGD.Visibility = Visibility.Hidden;
            old_validate.Visibility = new_validate.Visibility = Visibility.Hidden;
        }

        private void Set_GUI_Controls()
        {
            GUIControls.SecureArea.Set_GUI_Controls(this, otherControls);
            Username.FontSize = ActualWidth / 50;

            LoadProfile();
        }

        private void LoadProfile()
        {
            Person activeUser = Globals.GetActiveUser();

            Username.Content = activeUser.Username;

            username.Content = activeUser.Username[0]+"*****"+activeUser.Username[activeUser.Username.Length-1];
            firstname.Content = activeUser.Firstname;
            lastname.Content = activeUser.Lastname;
            email.Content = activeUser.Email[0] + "**********gmail.com";
            cellphone.Content = activeUser.Cellphone;
            password.Content = "************";
            control.Content = activeUser.Username == DatabaseSQL.GetFirstUser() ? "Total Control" : "Only control this account"; 
        }
       
        private void ChangeRequested()
        {
            changeRequestGD.Visibility = Visibility.Visible;
            Submit.Visibility = Visibility.Visible;
        }

        private void ChangeRequestComplete(string change)
        {
            changeRequestGD.Visibility = Visibility.Hidden;
            
            Globals.RefreshActiveUser();
            LoadProfile();

            MessageBox.Show($"You successfully changed {change}");
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (firstRun)
            {
                UserControl uc1 = new UserControl();
                mainGrid.Children.Add(uc1);
                window = Window.GetWindow(uc1);

                GUIControls.SecureArea.Set_FirstRun_Controls(this, window);

                firstRun = false;
            }

            Set_GUI_Controls();

        }
        
        /// <summary>
        /// Event Handler for MenuItems Clicked/Selected
        /// </summary>
        /// <param name="sender"> Clicked option</param>
        /// <param name="e"> Mouse Down</param>
        private void MenuItemClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label label = (Label)sender;

            Navigation.MenuItems.Navigate(this, label.Name, window);
        }
        
        private void NavigationItemsClicked(object sender, MouseButtonEventArgs e)
        {
            Border bd = (Border)sender;

            if (bd.Name == "acc_settings")
                MessageBox.Show("This is account settings");
            else
                Navigation.MainNavigation.Navigate(this, bd.Name, window);
        }

        private void changeProfilePic_Click(object sender, RoutedEventArgs e)
        {
            AppServices.ChangeProfilePicture();
        }

        private void ChangeRequestClick(object sender, RoutedEventArgs e)
        {
            ChangeRequested();

            Button click = (Button)sender;
            switch(click.Name)
            {
                case "emailChange":
                    old_lbl.Content = "Old Email";
                    new_lbl.Content = "New Email";
                    changeButton = Changes.Email;
                    break;

                case "cellphoneChange":
                    old_lbl.Content = "Old Cellphone";
                    new_lbl.Content = "New Cellphone";
                    changeButton = Changes.Cellphone;
                    break;

                case "passwordChange":
                    old_lbl.Content = "Old Password";
                    new_lbl.Content = "New Password";
                    changeButton = Changes.Password;
                    break;
            }
        }
        
        private bool InvalidEmail(string errMessage, Label validate)
        {
            validate.Visibility = Visibility.Visible;
            validate.Content = errMessage;
            return false;
        }

        private bool ValidEmail()
        {
            string oldEmail = oldField.Text.Trim().ToLower();
            if (oldEmail != Globals.GetActiveUser().Email)
            {
                return InvalidEmail("This email is does not match the active account!",old_validate);
            }

            string newEmail = newField.Text.Trim().ToLower();

            if(newEmail == "")
            {
                return InvalidEmail("Email cannot be empty", new_validate);
            }
            else if(!newEmail.Contains("@"))
            {
                return InvalidEmail("Email in incorrect format!", new_validate);
            }
            else if(!DatabaseSQL.IsEmailAvailable(newEmail))
            {
                return InvalidEmail("This email already exist!", new_validate);
            }

            return true;
        }

        private bool ValidCellphone(Person currentUser)
        {
            string oldtel = oldField.Text.Trim();
            string newtel = newField.Text.Trim();
            if (oldtel != currentUser.Cellphone)
            {
                old_validate.Visibility = Visibility.Visible;
                old_validate.Content = "This number is incorrect";
                return false;
            }
            if (newtel == currentUser.Cellphone)
            {
                new_validate.Visibility = Visibility.Visible;
                new_validate.Content = "The new number must be different to old one";
                return false;
            }
            
            return true;
        }

        private bool ValidPassword(Person currentUser)
        {
            string oldpass = oldField.Text.Trim();
            string newpass = newField.Text.Trim();
            if (oldpass != currentUser.Password)
            {
                old_validate.Visibility = Visibility.Visible;
                old_validate.Content = "This password is incorrect";
                return false;
            }
            if (newpass == currentUser.Password)
            {
                new_validate.Visibility = Visibility.Visible;
                new_validate.Content = "The new Password must be different to old one";
                return false;
            }

            return true;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Person currentUser = Globals.GetActiveUser();
            switch(changeButton)
            {
                case Changes.Cellphone:
                    if (ValidCellphone(currentUser))
                    {
                        old_validate.Visibility = Visibility.Hidden;
                        new_validate.Visibility = Visibility.Hidden;

                        string newtel = newField.Text.Trim();
                        DatabaseSQL.UpdateUser("Cellphone", newtel, currentUser.Username);
                        ChangeRequestComplete("Cellphone");
                    }
                    break;

                case Changes.Email:
                    if (ValidEmail())
                    {
                        old_validate.Visibility = Visibility.Hidden;
                        new_validate.Visibility = Visibility.Hidden;

                        string newEmail = newField.Text.Trim().ToLower();
                        DatabaseSQL.UpdateUser("Email", newEmail, currentUser.Username);
                        ChangeRequestComplete("Email");
                    }
                    break;

                case Changes.Password:
                    if (ValidPassword(currentUser))
                    {
                        old_validate.Visibility = Visibility.Hidden;
                        new_validate.Visibility = Visibility.Hidden;

                        string newPass = newField.Text.Trim().ToLower();
                        DatabaseSQL.UpdateUser("Password", newPass, currentUser.Username);
                        ChangeRequestComplete("Password");
                    }
                    break;
                    
            }
        }
    }
}
