using Folder_Locker.Database;
using Folder_Locker.Layouts;
using Folder_Locker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Folder_Locker.ViewModel
{
    class FolderViewModel
    {
        public static ObservableCollection<Folder> Folders { get; set; }

        public FolderViewModel()
        {
            Folders = new ObservableCollection<Folder>();

            LoadFoldersFromDB();
        }

        private void LoadFoldersFromDB()
        {
            string query = $"SELECT * FROM Folders WHERE Owner = '{Globals.GetActiveUsername()}'";
            DataTable table = DatabaseSQL.GetDataTable(query);

            for(int i=0; i<table.Rows.Count; i++)
            {
                Folder folder = new Folder
                {
                    FolderName = DatabaseSQL.DecodeName(table.Rows[i]["FolderName"].ToString()),
                    FolderPath = DatabaseSQL.DecodeName(table.Rows[i]["FolderPath"].ToString()),
                    FolderStatus = table.Rows[i]["FolderStatus"].ToString(),
                    FolderID = Convert.ToInt32(table.Rows[i]["FolderID"].ToString())
                };

                if(table.Rows[i]["FolderStatus"].ToString()=="UNLOCKED")
                {
                    folder.ImageSource = ICONS.Locks.GetLockOpenIconSource();
                }
                else
                {
                    folder.ImageSource = ICONS.Locks.GetLockClosedIconSource();
                }

                Folders.Add(folder);
            }
        }
        
        public void RemoveFolder(Folder folder)
        {
             Folders.Remove(folder);
        }

        public static Folder GetFolder(string name)
        {
            return Folders.FirstOrDefault(x => x.FolderName == name);
        }

        public void ChangeFolderStatus(string name)
        {
            Folder folder = Folders.FirstOrDefault(x => x.FolderName == name);

            if (folder.FolderStatus=="LOCKED")
            {
                folder.FolderStatus = "UNLOCKED";
                folder.ImageSource = "../Resources/Images/Locks/unlocked-icon.png";
            }
            else
            {
                folder.FolderStatus = "LOCKED";
                folder.ImageSource = "../Resources/Images/Locks/locked-icon.png";
            }
        }

        public void AddFolder(Folder newFolder)
        {
            Folders.Add(newFolder);
        }
    }
}
