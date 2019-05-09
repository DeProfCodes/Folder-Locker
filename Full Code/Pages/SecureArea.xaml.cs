// system libraries or namespaces
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Folder_Locker.Styles;
using Folder_Locker.ViewModel;
using Folder_Locker.Model;
using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;
using System.Collections.Generic;
using System.Diagnostics;

// User defined namespaces
using Folder_Locker.Database;
using Folder_Locker.Layouts;
using Folder_Locker.Views;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using Folder_Locker.Services;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for SecureArea.xaml
    /// </summary>
    public partial class SecureArea : Page
    {
        private bool firstRun;                              // To indicate first time page loads
        private Window window;                              // To represent the current page's window properties
        private FolderViewModel folderViewModelObject;      // ViewMoldel to list folders owned by a user
        private Folder selectedFolder;                      // Instance of selected folder

        private List<Image> pageIcons;                      // List all image icons that needs their sources to be set (by third party) 
        private List<Border> foldericonContainers;          // List of some boarder containing other icons (their activity (enable or disabled => toggle) needs to be set by third party 
        private List<object> otherControls;                 // Other controls that were stand alone and were not suitable to be created a style for them (third party styles them)
        
        /// <summary>
        /// Constructor to initialise GUI as well as call 'StartUps()' function to display the page
        /// </summary>
        public SecureArea()
        {
            InitializeComponent();
            StartUps();
        }
        
        /// <summary>
        /// Instatiate all objects, manage any user defined event handlers properly
        /// </summary>
        public void StartUps()
        {
            /////////////////////////////////////////////////////////////// TEMP LOGIN BYPASS ////////////////////////////////////////////////////////////////////
            //string username = DatabaseSQL.ResolveEmail_To_Username("p@gmail.com");
            //Globals.Set_ActiveUser(DatabaseSQL.GetUser(username));
            //Globals.Set_LoggedIn(true);
           
            /////////////////////////////////////////////////////////////// TEMP LOGIN BYPASS //////////////////////////////////////////////////////////////////// 

            firstRun = true;
            folderViewModelObject = new FolderViewModel();
            
            // Collect controls in grouped Lists
            pageIcons = new List<Image>
            {
                add_newFolder_ico, f_remove_ico,
                f_lock_ico, f_unlock_ico,
                manageAcc_icon, removeAcc_icon,
                accSettings_icon, exit_icon
            };
            foldericonContainers = new List<Border> { lock_folder, unlock_folder, remove_folder };
            otherControls = new List<object> { subGrid1, profile_ico, changeProfilePic, operationStatus };

           // profile_ico.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Images/ProfilePic.png")));
            // Third party to setup Icons accordingly
            ICONS.SecureArea.Set_StartUpIcons(this, pageIcons, profile_ico);                  
            
            ShowsNavigationUI = false;      // Disallow back navigation

            Username.Content = Globals.GetActiveUser().Username;
            selectedFolder = new Folder();

            // Hide some controls that will appear when certain events happen
            operationStatus.Visibility = Visibility.Hidden;

        }
        
        public void OperationStatusMessage(string action, string folderName)
        {
            operationStatus.Content = $"You have successfully {action} '{folderName}' folder";
            operationStatus.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Load up ViewMoldel into View, MVVM in effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            folderViewControl.DataContext = folderViewModelObject;
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

                // Temp
                //window.Width = ActualWidth * .75;

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

        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
            }
        }

         /// <summary>
/// Function to deal with request made for adding a new folder to user's list of folders owned
/// </summary>
        private void AddNewFolder()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            string path = "";
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                path = dialog.SelectedPath;
            }

            string name = path.Split('\\')[path.Split('\\').Length - 1];

            if (name != "")
            {
                Folder newFolder = new Folder
                {
                    FolderName = name,
                    FolderPath = path,
                    FolderStatus = "UNLOCKED",
                    ImageSource = ICONS.Locks.GetLockOpenIconSource()
                };

                Security.CreateLockerFolder(newFolder.FolderPath);

                bool folderInserted = DatabaseSQL.InsertFolderRecord(newFolder);

                if (folderInserted)
                {
                    folderViewModelObject.AddFolder(DatabaseSQL.GetLastFolder());
                    OperationStatusMessage("Added", name);
                }

                MessageBoxResult openFolder = MessageBox.Show("Folder added! Now you have to move the contents of this folder you want to protect into the folder named LOCKER which has just been added.\n" +
                                                                "Would you like to open folder directory now?", "Open folder directory", MessageBoxButton.YesNo);
                if(openFolder == MessageBoxResult.Yes)
                {
                    OpenFolder(newFolder.FolderPath);
                }
            }
        }

        /// <summary>
        /// Function to deal with request made for remove an existing to user's list of folders owned
        /// </summary>
        private void RemoveFolder()
        {
            Folder victim = Globals.GetCurrentFolder();
            folderViewModelObject.RemoveFolder(victim);

            DatabaseSQL.RemoveFolderRecord(victim.FolderID);

            OperationStatusMessage("Removed", victim.FolderName);
        }

        /// <summary>
        /// Function to deal with request made for locking a user owned folder
        /// </summary>
        private void LockFolder()
        {
            // GUI Update
            Folder folder = Globals.GetCurrentFolder();
            try
            {
                Security.LockFolder(folder.FolderPath);
            }
            catch
            {
                MessageBoxResult deleteFolder = MessageBox.Show("This folder does not exist in this machine, would you like to delete it from the database?", "Delete Folder", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(deleteFolder == MessageBoxResult.Yes)
                {
                    RemoveFolder();
                    return;
                }
            }
            // if try succeed, then finish up
            folderViewModelObject.ChangeFolderStatus(folder.FolderName);

            DatabaseSQL.UpdateFolderStatus(folder.FolderID, "LOCKED");
            
            OperationStatusMessage("Locked", folder.FolderName);
        }

        /// <summary>
        /// Function to deal with request made for unlocking a user owned folder
        /// </summary>
        private void UnlockFolder()
        {
            // GUI Update
            Folder folder = Globals.GetCurrentFolder();
            try
            {
                Security.UnlockFolder(folder.FolderPath);
            }
            catch
            {
                MessageBoxResult deleteFolder = MessageBox.Show("This folder does not exist in this machine, would you like to delete it from the database?", "Delete Folder", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (deleteFolder == MessageBoxResult.Yes)
                {
                    RemoveFolder();
                    return;
                }
            }
            folderViewModelObject.ChangeFolderStatus(folder.FolderName);

            DatabaseSQL.UpdateFolderStatus(folder.FolderID, "UNLOCKED");
            
            OperationStatusMessage("Unlocked", folder.FolderName);
        }

        /// <summary>
        /// Icons click, add new folder, remove folder, lock and unlock 'Buttons'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string clicked = ((Border)sender).Name;

            switch(clicked)
            {
                case "add_new_folder":
                    AddNewFolder();
                    selectedFolder = null;
                    ICONS.SecureArea.Reset_FolderIcons(this, pageIcons, foldericonContainers);
                    break;

                case "remove_folder":
                    RemoveFolder();
                    selectedFolder = null;
                    ICONS.SecureArea.Reset_FolderIcons(this, pageIcons, foldericonContainers);
                    break;

                case "lock_folder":
                    LockFolder();
                    ICONS.SecureArea.Toggle_FolderIcons(this, pageIcons, foldericonContainers);
                    break;

                case "unlock_folder":
                    UnlockFolder();
                    ICONS.SecureArea.Toggle_FolderIcons(this, pageIcons, foldericonContainers);
                    break;
            }
        }

        /// <summary>
        /// Navigation 'buttons' click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationItemsClicked(object sender, MouseButtonEventArgs e)
        {
            Border bd = (Border)sender;

            Navigation.MainNavigation.Navigate(this, bd.Name, window);
        }

        /// <summary>
        /// Event handler for folder select in the folderList scroll view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderViewControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Globals.IsFolderSelected)
            {
                if (selectedFolder == Globals.GetCurrentFolder())
                {
                    Globals.Set_FolderSelectedProperty(false);
                    return;
                }
                ICONS.SecureArea.Toggle_FolderIcons(this, pageIcons, foldericonContainers);
                selectedFolder = Globals.GetCurrentFolder();
            }
        }

        private void changeProfilePic_Click(object sender, RoutedEventArgs e)
        {
            AppServices.ChangeProfilePicture();
        }
    }
}
