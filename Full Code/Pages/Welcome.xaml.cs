using Folder_Locker.Layouts;
using Folder_Locker.Services;
using Folder_Locker.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Folder_Locker.Pages
{
    /// <summary>
    /// Interaction logic for Welcomee.xaml
    /// </summary>
    public partial class Welcome : Page
    {
        private bool firstRun;
        public Window window;

        List<object> controls;
        
        public Welcome()
        {
            InitializeComponent();

            StartUps();
        }
        

        private void StartUps()
        {
            firstRun = true;

            ShowsNavigationUI = false;

            controls = new List<object>
            {
                login, new_admin, appName, companyName, folderICO, companyLogo
            };

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (firstRun)
            {
                WelcomeStyles.Set_StaticStyles(ActualWidth, this);

                UserControl uc1 = new UserControl();
                mainGrid.Children.Add(uc1);
                window = Window.GetWindow(uc1);

                GUIControls.Welcome.Set_FirstRun_Controls(this, window);
                
                firstRun = false;
            }

            WelcomeStyles.Set_DynamicStyles(ActualWidth, this);
            GUIControls.Welcome.Set_GUI_Controls(this, controls);
        }

        private void new_admin_Click(object sender, RoutedEventArgs e)
        {
            Navigation.GotoSignUp(this);
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn(window, this);
            logIn.Show();
            logIn.Owner = window;
            window.Hide();
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
    }
}
