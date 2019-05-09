// User defined Libraries
using Folder_Locker.Database;
using Folder_Locker.Model;
using Folder_Locker.Services;
using Folder_Locker.Styles;
using Folder_Locker.ViewModel;
using Folder_Locker.Layouts;

// System Libraries 
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for EditAccount.xaml
    /// </summary>
    public partial class EditAccount : Page
    {
        private bool firstRun;                              // To indicate first time page loads
        private Window window;                              // To represent the current page's window properties
       
        private List<Image> pageIcons;                      // List all image icons that needs their sources to be set (by third party) 
        private List<object> otherControls;                 // Other controls that were stand alone and were not suitable to be created a style for them (third party styles them)
    
        public EditAccount()
        {
            InitializeComponent();
            StartUps();
        }
        
        /// <summary>
        /// Instatiate all objects, manage any user defined event handlers properly
        /// </summary>
        public void StartUps()
        {
            firstRun = true;
           
            // Collect controls in grouped Lists
            pageIcons = new List<Image>
            {
                manageAcc_icon,
                home_icon, exit_icon
            };

            otherControls = new List<object> { subGrid1, profile_ico, changeProfilePic};

            // Third party to setup Icons accordingly
            ICONS.SecureArea.Set_StartUpIcons(this, pageIcons, profile_ico);

            ShowsNavigationUI = true;      // Allow back navigation

            Username.Content = Globals.GetActiveUser().Username;
            
            LoadCurrentAccountDetails();
            
        }
        
        /// <summary>
        /// Load the 'Selected' account details and display them in a texblock assign for that purpose
        /// </summary>
        private void LoadCurrentAccountDetails()
        {
            Person account = Globals.GetSelectedAccount();

            firstname_cell.Text = account.Firstname;
            lastname_cell.Text = account.Lastname;
            email_cell.Text = account.Email;
            cellphone_cell.Text = account.Cellphone;
            password_cell.Text = account.Password;

            DisplaySelectedAccountInfo(account);
        }

        /// <summary>
        /// Event handler for change in screen size (width or height or both) overtime, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (firstRun)
            {
                SecureAreaStyles.SetStyles(ActualWidth, this);                  // Set static styles
                SignUpStyles.Set_StaticStyles(ActualWidth, this);

                UserControl uc1 = new UserControl();
                mainGrid.Children.Add(uc1);
                window = Window.GetWindow(uc1);

                GUIControls.SecureArea.Set_FirstRun_Controls(this, window);     // Set Window properties 
                
                firstRun = false;
            }

            GUIControls.SecureArea.Set_GUI_Controls(this, otherControls); // Set Dynamic styles
            SignUpStyles.EntryCells(ActualWidth, this);
            SignUpStyles.LabelZForCells(ActualWidth, this);
        }
        
        /// <summary>
        /// Event Handler for MenuItems Clicked/Selected
        /// </summary>
        /// <param name="sender"> Clicked option</param>
        /// <param name="e"> Mouse Down</param>
        private void MenuItemsClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Label label = (Label)sender;

            Navigation.MenuItems.Navigate(this, label.Name, window);
        }
        
        /// <summary>
        /// Event handler for any button navigation present in the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationItemsClicked(object sender, MouseButtonEventArgs e)
        {
            Border bd = (Border)sender;
            Navigation.MainNavigation.Navigate(this, bd.Name, window);
        }

        /// <summary>
        /// Profile Picture change request!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeProfilePic_Click(object sender, RoutedEventArgs e)
        {
            AppServices.ChangeProfilePicture();
        }

        /// <summary>
        /// Method that given a Person struct, decomposes and display the contents in a textblock
        /// </summary>
        /// <param name="selectedAccount"></param>
        private void DisplaySelectedAccountInfo(Person selectedAccount)
        {
            accountInfo_lbl.Visibility = Visibility.Visible;
            accountInfo.Text = $"   : {selectedAccount.Username}\n" +
                               $"   : {selectedAccount.Firstname}\n" +
                               $"   : {selectedAccount.Lastname}\n" +
                               $"   : {selectedAccount.Gender}\n" +
                               $"   : {selectedAccount.Cellphone}\n" +
                               $"   : {selectedAccount.Email}\n" +
                               $"   : {selectedAccount.Password}";
        }
        
        /// <summary>
        /// Submit button to apply new edited details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeRequest_Click(object sender, RoutedEventArgs e)
        {
            /// PRE-CONDITIONS
            /// First check if there was a change before contacting the Database
            /// Change is done by contacting the Database and do an UPDATE query to the database
             
            Person account = Globals.GetSelectedAccount();

            if(account.Firstname !=firstname_cell.Text.Trim() && AppValidations.SignUp.IsFirstnameValid(firstname_cell.Text))
                DatabaseSQL.UpdateUser("Firstname", firstname_cell.Text, account.Username);

            else if(!AppValidations.SignUp.IsFirstnameValid(firstname_cell.Text))
            {
                MessageBox.Show("Firstname cannot contain illegal symbols or empty!");
                return;
            }
            
            if(account.Lastname != lastname_cell.Text.Trim())
                DatabaseSQL.UpdateUser("Lastname", lastname_cell.Text, account.Username);

            string invalidEmail_err = AppValidations.SignUp.IsEmailValid(this, email_cell.Text);

            if (account.Email != email_cell.Text.Trim() &&  invalidEmail_err== "")
                DatabaseSQL.UpdateUser("Email", email_cell.Text, account.Username);

            else if (invalidEmail_err != "")
            {
                MessageBox.Show(invalidEmail_err);
                return;
            }

            if (account.Cellphone != cellphone_cell.Text.Trim())
                DatabaseSQL.UpdateUser("Cellphone", cellphone_cell.Text, account.Username);

            if (account.Password != password_cell.Text)
                DatabaseSQL.UpdateUser("Password", password_cell.Text, account.Username);

            AccountViewModel.LoadAccountsFromDB();      // Refresh the ViewModel in order to display updated account details

            // If current user details change, update the currentUser details
            if(account.Username == Globals.GetActiveUser().Username)
            {
                Globals.Set_ActiveUser(DatabaseSQL.GetUser(account.Username));
            }

            if(!Globals.AccountComparison(account, DatabaseSQL.GetUser(account.Username)))
            {
                Navigation.GotoManageAccounts(this);
                MessageBox.Show("You have successfully updated account details!");
            }
            else
            {
                MessageBoxResult act = MessageBox.Show("No change was done, continue editing?", "No change", MessageBoxButton.YesNo);
                if(act == MessageBoxResult.No)
                {
                    Navigation.GotoManageAccounts(this);
                }
            }
        }
    }
}
