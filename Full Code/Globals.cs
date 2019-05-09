using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Folder_Locker.Database;
using Folder_Locker.Model;

namespace Folder_Locker
{
    public static class Globals
    {
        public static bool IsFolderSelected { get; set; }
        private static Folder CurrentFolder;
        
        public static bool IsAccountSelected { get; set; }
        private static Person SelectedAccount;

        private static Person ActiveUser;
        
        public enum LoginStatus {Succeeded, Failed };
        private static Enum loginStatus = LoginStatus.Failed;

        private static Point Dimensions = new Point(-1,-1);

        #region Dimensions
        public static void Set_Dimesions(double width, double height)
        {
            Dimensions.X = width;
            Dimensions.Y = height;
        }

        public static Point GetDimensions()
        {
            return Dimensions;
        }
        #endregion

        #region FolderSelected
        public static void Set_FolderSelectedProperty(bool isFolderSelected)
        {
            IsFolderSelected = isFolderSelected;
        }

        public static void SetCurrentFolder(Folder folder)
        {
            CurrentFolder = folder;
        }
       
        public static Folder GetCurrentFolder()
        {
            return CurrentFolder;
        }
        #endregion

        #region Selected Account
        public static void Set_AccountSelectedProperty(bool isAccountSelected)
        {
            IsAccountSelected = isAccountSelected;
        }

        public static void SetSelectedAccount(Person selectedAccount)
        {
            SelectedAccount = selectedAccount;
        }

        public static Person GetSelectedAccount()
        {
            return SelectedAccount;
        }
        #endregion

        #region Active/LoggedIn User
        public static void Set_ActiveUser(Person user)
        {
            ActiveUser = user;
        }

        public static string GetActiveUsername()
        {
            return ActiveUser.Username;
        }

        public static Person GetActiveUser()
        {
            return ActiveUser;
        }

        public static void RefreshActiveUser()
        {
            ActiveUser = DatabaseSQL.GetUser(GetActiveUsername());
        }
        #endregion

        #region Log in
        public static void Set_LoggedIn(bool access)
        {
            loginStatus = access ? LoginStatus.Succeeded : LoginStatus.Failed;
        }

        public static Enum GetLogInOutcome()
        {
            return loginStatus;
        }
        #endregion

        public static bool AccountComparison(Person person1, Person person2)
        {
            return (
                     person1.Username == person2.Username &&
                    person1.Firstname == person2.Firstname &&
                     person1.Lastname == person2.Lastname &&
                        person1.Email == person2.Email &&
                    person1.Cellphone == person2.Cellphone &&
                     person1.Password == person2.Password
                    );
        }
    }
}
