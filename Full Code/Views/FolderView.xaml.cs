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

namespace Folder_Locker.Views
{
    /// <summary>
    /// Interaction logic for FolderView.xaml
    /// </summary>
    public partial class FolderView : UserControl
    {
        private static List<object> All_Labels;

        public FolderView()
        {
            InitializeComponent();

            All_Labels = new List<object>();
          //  IsFolderSelected = false;
        }
        

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
        }
        
        public static void UnselectRow()
        {
            for(int i=0;i<All_Labels.Count;i++)
            {
                if(All_Labels[i] is Label)
                {
                    Label label = (Label)All_Labels[i];
                    label.Background = null;
                }
                else
                {
                    StackPanel bdr = (StackPanel)All_Labels[i];
                    bdr.Background = null;
                }
            }
           
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UnselectRow(); 

            Border bd = (Border)sender;
            Grid grid = (Grid)bd.Child;
            
            StackPanel sp1 = (StackPanel)grid.Children[0];
            
            Label path = (Label)grid.Children[2];

            StackPanel sp2 = (StackPanel)grid.Children[4];
            
            path.Background = Brushes.LightGreen;
            sp1.Background = Brushes.LightGreen;
            sp2.Background = Brushes.LightGreen;

            if (!All_Labels.Contains(sp1)) All_Labels.Add(sp1);
            if (!All_Labels.Contains(path)) All_Labels.Add(path);
            if (!All_Labels.Contains(sp2)) All_Labels.Add(sp2);

            Globals.Set_FolderSelectedProperty(true);

            Label fname = (Label)sp1.Children[1];

            Model.Folder folder = ViewModel.FolderViewModel.GetFolder((string) fname.Content);
            Globals.SetCurrentFolder(folder);
        }
        
    }
}
