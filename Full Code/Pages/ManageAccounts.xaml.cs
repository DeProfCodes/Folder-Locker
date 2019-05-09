using Folder_Locker.Database;
using Folder_Locker.Layouts;
using Folder_Locker.Model;
using Folder_Locker.Services;
using Folder_Locker.Styles;
using Folder_Locker.ViewModel;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for ManageAccounts.xaml
    /// </summary>
    public partial class ManageAccounts : Page
    {
        private bool firstRun;                              // To indicate first time page loads
        private Window window;                              // To represent the current page's window properties
        private AccountViewModel accountViewModelObject;    // ViewMoldel to list folders owned by a user
       
        private List<Image> pageIcons;                      // List all image icons that needs their sources to be set (by third party) 
       
        private List<object> otherControls;                 // Other controls that were stand alone and were not suitable to be created a style for them (third party styles them)
        private Person selectedAccount;                     // Instance of selected account


        public ManageAccounts()
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
            accountViewModelObject = new AccountViewModel();

            // Collect controls in grouped Lists
            pageIcons = new List<Image>
            {
                manageAcc_icon, removeAcc_icon,
                accSettings_icon, exit_icon
            };

            otherControls = new List<object> { subGrid1, profile_ico, changeProfilePic};
            
            ICONS.SecureArea.Set_StartUpIcons(this, pageIcons, profile_ico);

            ShowsNavigationUI = true;      // Allow back navigation

            Username.Content = Globals.GetActiveUser().Username;

            selectedAccount = new Person();

            //Hide some controls to be visible once an account is selected
            accountInfo_lbl.Visibility = Visibility.Hidden;
            editAccount.Visibility = deleteAccount.Visibility = Visibility.Hidden;

        }
        
        /// <summary>
        /// Load up ViewMoldel into View, MVVM in effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            accountViewControl.DataContext = accountViewModelObject;
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

                UserControl uc1 = new UserControl();
                mainGrid.Children.Add(uc1);
                window = Window.GetWindow(uc1);

                GUIControls.SecureArea.Set_FirstRun_Controls(this, window);     // Set Window properties 

                firstRun = false;
            }

            GUIControls.SecureArea.Set_GUI_Controls(this, otherControls); // Set Dynamic styles
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
        
        /// <summary>
        /// Event handler for any navigation button clicked in this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationItemsClicked(object sender, MouseButtonEventArgs e)
        {
            Border bd = (Border)sender;
            switch (bd.Name)
            {
                case "manage_accs":
                    MessageBox.Show("This is managae accounts!");
                    break;
    
                case "deleteAccount":
                    AppServices.RemoveAccount.DeleteAccount(this, Globals.GetSelectedAccount().Username);
                    accountInfo.Text = "Select account to display full information";
                    deleteAccount.Visibility = editAccount.Visibility = accountInfo_lbl.Visibility = Visibility.Hidden;
                    break;

                default:
                    Navigation.MainNavigation.Navigate(this, bd.Name, window);
                    break;
            }

        }

        /// <summary>
        /// Change profile picture of account method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeProfilePic_Click(object sender, RoutedEventArgs e)
        {
            AppServices.ChangeProfilePicture();
        }

        /// <summary>
        /// Given an account (Person) decompose the structure and display its content in textblock reserved for this purpose
        /// </summary>
        /// <param name="selectedAccount"></param>
        private void DisplaySelectedAccountInfo(Person selectedAccount)
        {
            accountInfo_lbl.Visibility = Visibility.Visible;
            editAccount.Visibility = deleteAccount.Visibility = Visibility.Visible;

            accountInfo.Text = $"   : {selectedAccount.Username}\n" +
                               $"   : {selectedAccount.Firstname}\n" +
                               $"   : {selectedAccount.Lastname}\n" +
                               $"   : {selectedAccount.Gender}\n" +
                               $"   : {selectedAccount.Cellphone}\n" +
                               $"   : {selectedAccount.Email}\n" +
                               $"   : {selectedAccount.Password}";
        }

        /// <summary>
        /// Event handler for mouse click in the account list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountViewControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Globals.IsAccountSelected)
            {
                if (selectedAccount == Globals.GetSelectedAccount())
                {
                    Globals.Set_AccountSelectedProperty(false);
                    return;
                }
                
                selectedAccount = Globals.GetSelectedAccount();

                DisplaySelectedAccountInfo(selectedAccount);
            }
            
        }
    }
}
