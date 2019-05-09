using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using Folder_Locker.Model;
using Folder_Locker.Pages;
using Folder_Locker.ViewModel;
using Folder_Locker.Layouts;

namespace Folder_Locker.Database
{
    public static class DatabaseSQL
    {
        private static bool delayedConnection = false;

        private static string BuildPath(string absolute)
        {
            string path = "";
            string[] directries = absolute.Split('\\');
            for(int i=1;i<directries.Length;i++)
            {
                path += directries[i] + "\\";
            }

            return path.Substring(0, path.Length - 1);
        }

        private static SqlConnection GetConnection()
        {
            string pathD = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            string connectionStr = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={BuildPath(pathD)}\\Data\\LockerDB.mdf;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionStr);
         
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                    return conn;
                }
                catch (Exception e)
                {
                    GetConnection();
                    if (!delayedConnection)
                        MessageBox.Show("Please wait");
                    delayedConnection = true;
                }
            }

            return conn;
        }

        public static DataTable GetDataTable(string SQL_Query)
        {
            SqlConnection conn = GetConnection();

            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SQL_Query, conn);
            adapter.Fill(table);
            
            return table;
        }

        private static void ExecuteSQL(string sqlQuery)
        {
            SqlConnection conn = GetConnection();

            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            cmd.ExecuteNonQuery();
        }

        #region FOLDERS 
        private static bool FolderIsOwned(Folder folder)
        {
            string query = "SELECT * FROM Folders";
            DataTable table = GetDataTable(query);

            string folderTrueID = folder.FolderName + folder.FolderPath;

            for(int i=0;i<table.Rows.Count; i++)
            {
                string fID = table.Rows[i]["FolderName"].ToString() + table.Rows[i]["FolderPath"].ToString();

                if (folderTrueID == fID) return true;
            }

            return false;
        }

        private static string GetFolderOwner(Folder folder)
        {
            string query = "SELECT * FROM Folders";
            DataTable table = GetDataTable(query);

            string folderTrueID = folder.FolderName + folder.FolderPath;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string fID = table.Rows[i]["FolderName"].ToString() + table.Rows[i]["FolderPath"].ToString();

                if (folderTrueID == fID) return table.Rows[i]["Owner"].ToString();
            }

            return "";
        }

        public static string DecodeName(string name)
        {
            while (name.Contains('*'))
            {
                int idx = name.IndexOf('*');
                name = name.Substring(0, idx) + "'" + name.Substring(idx + 1);
            }
            return name;
        }

        public static Folder GetLastFolder()
        {
            string query = $"SELECT TOP 1 * FROM Folders ORDER BY FolderId DESC";

            DataTable folderRow = GetDataTable(query);

            if(folderRow.Rows.Count!=0)
            {
                Folder last = new Folder
                {
                    FolderID = Convert.ToInt32(folderRow.Rows[0]["FolderID"].ToString()),
                    FolderName = DecodeName(folderRow.Rows[0]["FolderName"].ToString()),
                    FolderPath = DecodeName(folderRow.Rows[0]["FolderPath"].ToString()),
                    FolderStatus = folderRow.Rows[0]["FolderStatus"].ToString(),
                    ImageSource = ICONS.Locks.GetLockOpenIconSource()
                };

                return last;
            }
            else
            {
                MessageBox.Show("Something went wrong in the function 'GetLastFolder()'");
                return null;
            }
        }

        private static string EncodeName(string name)
        {
            while(name.Contains('\''))
            {
                int idx = name.IndexOf('\'');
                name = name.Substring(0,idx)+"*"+name.Substring(idx+1);
            }
            return name;
        }

        public static bool InsertFolderRecord(Folder newFolder)
        {
            if(FolderIsOwned(newFolder))
            {
                string folderOwner = GetFolderOwner(newFolder);

                if (Globals.GetActiveUser().Username != folderOwner)
                {
                    MessageBox.Show($"Sorry, you cannot have ownership of this folder as it is owned by another user = {folderOwner}, Try another Folder!");
                }
                else
                {
                    MessageBox.Show("You already own this folder");
                }
                return false;
            } //&quot;Copy if New&quot;
            string insertIntoDB =
                     $"INSERT INTO Folders VALUES('{Globals.GetActiveUsername()}','{EncodeName(newFolder.FolderName)}','{EncodeName(newFolder.FolderPath)}','{newFolder.FolderStatus}')";

            ExecuteSQL(insertIntoDB);

            return true;
        }

        public static void RemoveFolderRecord(int FolderID)
        {
            string removeFromDB =
                   $"DELETE FROM Folders WHERE Owner='{Globals.GetActiveUsername()}' AND FolderID={FolderID}";
            ExecuteSQL(removeFromDB);
        }

        private static void RemoveFolderRecords(string username)
        {
            string removeFromDB =
                   $"DELETE FROM Folders WHERE Owner='{username}'";
            ExecuteSQL(removeFromDB);
        }

        public static void UpdateFolderStatus(int FolderID, string status)
        {
            string updateStatusDB =
                   $"UPDATE Folders " +
                       $"SET FolderStatus = '{status}'" +
                       $"WHERE Owner='{Globals.GetActiveUsername()}' AND FolderID={FolderID}";
            ExecuteSQL(updateStatusDB);
        }
        #endregion

        #region USERS
        public static bool IsUsernameAvailable(string username)
        {
            string query = $"SELECT Username FROM Users WHERE Username='{username}'";
            DataTable cell = GetDataTable(query);

            if (cell.Rows.Count == 0) return true;
            else return false;
        }

        public static bool CorrectCredentials(string username, string password)
        {
            string usernOrEmail = username.Contains("@") ? "Email" : "Username";
            string query = $"SELECT Password FROM Users WHERE {usernOrEmail} = '{username}'";
            

            DataTable cell = GetDataTable(query);

            return cell.Rows[0]["Password"].ToString() == password;
        }

        public static bool IsEmailAvailable(string email)
        {
            string query = $"SELECT Email FROM Users WHERE Email='{email}'";
            DataTable cell = GetDataTable(query);

            if (cell.Rows.Count == 0) return true;
            else return false;
        }

        public static string ResolveEmail_To_Username(string email)
        {
            string query = $"SELECT Username FROM Users WHERE Email = '{email}'";

            DataTable cell = GetDataTable(query);

            return cell.Rows[0]["Username"].ToString();
        }

        public static void InsertUserRecord(Person user)
        {
            string query =
                $"INSERT INTO Users VALUES ('{user.Username}'," +
                $"'{user.Firstname}','{user.Lastname}','{user.Email}'," +
                $"'{user.Cellphone}','{user.Password}','{user.Gender}')";

            ExecuteSQL(query);
        }

        public static void RemoveUserRecord(string Username)
        {
            string query =
                $"DELETE FROM Users WHERE Username ='{Username}'";

            RemoveFolderRecords(Username);

            ExecuteSQL(query);
        }

        public static Person GetUser(string username)
        {
            string query = $"Select * from Users Where Username = '{username}'";

            DataTable row = GetDataTable(query);

            if (row.Rows.Count != 0)
            {
                Person user = new Person
                {
                    Username = username,
                    Firstname = row.Rows[0]["Firstname"].ToString(),
                    Lastname = row.Rows[0]["Lastname"].ToString(),
                    Email = row.Rows[0]["Email"].ToString(),
                    Cellphone = row.Rows[0]["Cellphone"].ToString(),
                    Password = row.Rows[0]["Password"].ToString(),
                    Gender = row.Rows[0]["Gender"].ToString(),
                };

                return user;
            }
            else
            {
                MessageBox.Show($"Something went wrong :( No user matching username '{username}' was not found!");
                return null;
            }
        }

        public static string GetFirstUser()
        {
            string query = $"SELECT TOP 1 Username FROM Users ORDER BY userId";
            DataTable cell = GetDataTable(query);

            return cell.Rows[0]["Username"].ToString();
        }

        public static string GetSecondUser()
        {
            string query = $"SELECT Username FROM Users ORDER BY UserID";
            DataTable cell = GetDataTable(query);
            if (cell.Rows.Count < 2) return "";

            return cell.Rows[1]["Username"].ToString();
        }

        public static void UpdateUser(string field, string value, string username)
        {
            string query = $"UPDATE Users " +
                              $"Set {field} = '{value}'" +
                              $" Where Username = '{username}'";

            ExecuteSQL(query);
        }
        #endregion
    }
}
