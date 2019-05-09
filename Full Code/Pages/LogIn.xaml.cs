using Folder_Locker.Database;
using Folder_Locker.Services;
using Folder_Locker.Layouts;

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private bool firstRun;
        private bool usernameTxbFocus;
        private bool signUpBtnClick;

        private Window window;
        private Page page;

        private List<object> controls;
        private List<Label> errorLbls;
        private List<object> entryCells;

        public LogIn(Window window, Page page)
        {
            InitializeComponent();

            StartUps(window, page);
        }

        /// <summary>
        /// Event handler for closing (clicking Window's X)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            switch(Globals.GetLogInOutcome())
            {   // If login page is terminated (before login succeed) for some reason, close all widows that the application may have opened!
                case Globals.LoginStatus.Failed:
                    if(!signUpBtnClick) window.Close();
                    break;
            }
        }

        /// <summary>
        /// Function to Setup all appropriate controls for GUI and other startups required
        /// </summary>
        /// <param name="windowParent"></param>
        /// <param name="pageParent"></param>
        private void StartUps(Window windowParent, Page pageParent)
        {
            firstRun = true;
            usernameTxbFocus = false;
            signUpBtnClick = false;

            controls = new List<object>
            {
                username, password,
                username_lbl, password_lbl,
                username_validate, password_validate, missing_fileds_validate,
                reset_lbl1, reset_lbl2,
                Login, SignUp, heading, border
            };

            errorLbls = new List<Label>
            {
                username_validate, password_validate, missing_fileds_validate
            };

            entryCells = new List<object> { username, password };

            window = windowParent;          // parentWindow = (WelcomeScreen OR SignUp)
            page = pageParent;              // parentPage = (WelcomeScreen OR SignUp)

            Closing += Window_Closing;      // window close (X) event handler
        }
        
        /// <summary>
        /// Start Up GUI controls allignments and properties setup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (firstRun)
            {
                Width = 0.38 * ActualWidth;

                ResizeMode = ResizeMode.CanMinimize;

                Icon = ICONS.Folders.GetAppIcon();
                firstRun = false;
            }

            GUIControls.Login.Set_GUI_Controls(this, controls);
        }
       
        /// <summary>
        /// Event handler for login button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            /// PRE-CONDITIONS
            ///  Call upon Validations to verified entered information i.e Username and Password if are correct
            ///   Then display appropriate message should an error occur
           if(AppValidations.LogIn.CredentialsValidated(errorLbls, entryCells, page))
            {
                string userName = username.Text.Trim().ToLower();
                userName = userName.Contains("@") ? DatabaseSQL.ResolveEmail_To_Username(userName) : userName;

                Globals.Set_ActiveUser(DatabaseSQL.GetUser(userName));
                Globals.Set_LoggedIn(true);

                Navigation.GotoSecureArea(page);
                window.Show();
                Close();
            }

            Cursor = Cursors.Arrow;
        }
        
        /// <summary>
        /// Event handler for when TextBox "Username" gets focus (by being clicked)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!usernameTxbFocus)
            {
                username.Text = "";
                username.Foreground = Brushes.Black;
                username.FontStyle = FontStyles.Normal;
            }
            usernameTxbFocus = true;
        }

        /// <summary>
        /// Event handler for when the text "Click me" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Oh hey, I'm clicked!");
        }

        /// <summary>
        /// Event handler for Sign Up button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Navigation.GotoSignUp(page);
            window.Show();
            signUpBtnClick = true;
            Close();
        }
    }
}
