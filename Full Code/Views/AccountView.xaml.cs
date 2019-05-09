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

namespace Folder_Locker.Views
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        private List<StackPanel> rows;
        public AccountView()
        {
            InitializeComponent();

            rows = new List<StackPanel>();
        }

        private void UnselectRow()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].Background = null;
            }

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UnselectRow();

            Border bd = (Border)sender;
            
            StackPanel sp1 = (StackPanel)bd.Child;

            StackPanel sp2 = (StackPanel)sp1.Children[1];
            Label username = (Label)sp2.Children[0];

            Model.Person account = ViewModel.AccountViewModel.GetAccount((string)username.Content);
            Globals.Set_AccountSelectedProperty(true);
            Globals.SetSelectedAccount(account);

            sp1.Background = Brushes.LightGreen;
            
            if (!rows.Contains(sp1)) rows.Add(sp1);
           
        }
    }
}
