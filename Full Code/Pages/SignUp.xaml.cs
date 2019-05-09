using Folder_Locker.Database;
using Folder_Locker.Model;
using Folder_Locker.Services;
using Folder_Locker.Styles;
using Folder_Locker.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Folder_Locker.Layouts;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        private bool firstRun;
        private Window window;

        private List<object> entry_cells;
        private List<Label> errorLbls;
        private List<object> otherControls;

        public SignUp()
        {
            InitializeComponent();

            StartUps();
        }
        
        private void StartUps()
        {
            firstRun = true;

            entry_cells = new List<object>
            {
                username, firstname, email, password, passwordConfirm
            };

            errorLbls = new List<Label>
            {
                username_validate, email_validate, password_validate,
                firstname_validate, cellphone_validate, missing_fileds_validate
            };
            // hide error labels
            for (int i = 0; i < errorLbls.Count; i++)
                errorLbls[i].Visibility = Visibility.Hidden;

            otherControls = new List<object>
            {
                create_Account, heading, border, companyLogo,companyName, gender
            };

            ShowsNavigationUI = true;
        }
        
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (firstRun)
            {
                SignUpStyles.Set_StaticStyles(ActualWidth, this);

                UserControl uc1 = new UserControl();
                mainGrid.Children.Add(uc1);
                window = Window.GetWindow(uc1);

                GUIControls.SignUp.Set_FirstRun_Controls(this, window);
                
                firstRun = false;
            }

            SignUpStyles.SetDynamicStyles(ActualWidth, this);
            GUIControls.SignUp.Set_GUI_Controls(this, otherControls);
        }

        private void Create_Account_Click(object sender, RoutedEventArgs e)
        {
            bool emptyRequiredFields = AppValidations.RequiredFieldsEmpty(entry_cells, missing_fileds_validate);
            bool invalidFields = AppValidations.SignUp.IsFieldInvalid(this, entry_cells, errorLbls);

            bool validated = !(emptyRequiredFields || invalidFields);
            
            if(validated)
            {
                Person user = new Person
                {
                    Username = username.Text.Trim().ToLower(),
                    Firstname = firstname.Text,
                    Lastname = lastname.Text,
                    Email = email.Text.Trim().ToLower(),
                    Cellphone = cellphone.Text,
                    Password = password.Password,
                    Gender = gender.Text
                };

                DatabaseSQL.InsertUserRecord(user);
               
                switch(Globals.GetLogInOutcome())
                {
                    case Globals.LoginStatus.Succeeded:
                        Navigation.GotoManageAccounts(this);
                        MessageBox.Show($"A account with Username '{user.Username}' has been successfully created!");
                        break;

                    case Globals.LoginStatus.Failed:
                        MessageBox.Show($"A account with Username '{user.Username}' has been successfully created, proceed to Login");
                        LogIn logIn = new LogIn(window, this);
                        logIn.Show();
                        logIn.Owner = window;
                        window.Hide();
                        break;
                }
            }
        }
        
        /// <summary>
        /// Event Handler for MenuItems Clicked/Selected
        /// </summary>
        /// <param name="sender"> Clicked option</param>
        /// <param name="e"> Mouse Down</param>
        private void MenuItemClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            Navigation.MenuItems.Navigate(this,label.Name, window);
        }

    }
}
