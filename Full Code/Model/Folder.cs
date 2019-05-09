using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Folder_Locker.Model
{
    public class Folder : INotifyPropertyChanged
    {
        private string name;
        private string path;
        private string status;
        private string imgSrc;
        private int folderId;

        public string FolderName
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("FolderName");
                }
            }
        }

        public string FolderPath
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    RaisePropertyChanged("FolderPath");
                }
            }
        }

        public string FolderStatus
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChanged("FolderStatus");
                }
            }
        }

        public string ImageSource
        {
            get { return imgSrc; }
            set
            {
                if (imgSrc != value)
                {
                    imgSrc = value;
                    RaisePropertyChanged("ImageSource");
                }
            }
        }

        public int FolderID
        {
            get { return folderId; }
            set
            {
                if (folderId != value)
                {
                    folderId = value;
                    RaisePropertyChanged("FolderID");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        
    }
}
