using Folder_Locker.Database;
using Folder_Locker.Model;

using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Folder_Locker.ViewModel
{
    class AccountViewModel
    {
        public static ObservableCollection<Person> Accounts { get; set; }

        public AccountViewModel()
        {
            Accounts = new ObservableCollection<Person>();

            LoadAccountsFromDB();
        }
        
        public static void LoadAccountsFromDB()
        {
            Accounts.Clear();

            string query = "SELECT * FROM Users";
            DataTable table = DatabaseSQL.GetDataTable(query);

            for(int i=0;i<table.Rows.Count; i++)
            {
                Person user = new Person
                {
                    Username = table.Rows[i]["Username"].ToString(),
                    Firstname = table.Rows[i]["Firstname"].ToString(),
                    Lastname = table.Rows[i]["Lastname"].ToString(),
                    Email = table.Rows[i]["Email"].ToString(),
                    Cellphone = table.Rows[i]["Cellphone"].ToString(),
                    Gender = table.Rows[i]["Gender"].ToString(),
                    Password = table.Rows[i]["Password"].ToString()
                };

                Accounts.Add(user);
            }
        }
        
        public void AddAccount(Person user)
        {
            Accounts.Add(user);
        }

        public void RemoveAccount(Person user)
        {
            Accounts.Remove(user);
        }

        public static Person GetAccount(string username)
        {
            return Accounts.FirstOrDefault(x => x.Username == username);
        }

    }
}
