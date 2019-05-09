using Folder_Locker.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Folder_Locker.Services
{
    public static class AppValidations
    {
        private static void EmptyFiled(object cell, ref bool empty)
        {
            empty = true;
            if (cell is TextBox)
            {
                TextBox box = (TextBox)cell;
                box.BorderBrush = Brushes.Red;
            }
            else
            {
                PasswordBox box = (PasswordBox)cell;
                box.BorderBrush = Brushes.Red;
            }
        }

        private static void RequiredFiedls(List<object> entry_cells, ref bool emptyRF)
        {
            for (int i = 0; i < entry_cells.Count; i++)
            {
                if (entry_cells[i] is TextBox)
                {
                    TextBox entry = (TextBox)entry_cells[i];

                    if (entry.Text == "" || entry.Text == "Enter username or email")
                        EmptyFiled(entry, ref emptyRF);
                    else
                        entry.BorderBrush = Brushes.LightGray;
                }
                else
                {
                    PasswordBox entry = (PasswordBox)entry_cells[i];

                    if (entry.Password == "")
                        EmptyFiled(entry, ref emptyRF);
                    else
                        entry.BorderBrush = Brushes.LightGray;
                }
            }
        }

        public static bool RequiredFieldsEmpty(List<object> entry_cells, Label missingFieldsReport)
        {
            bool emptyRF = false;

            RequiredFiedls(entry_cells, ref emptyRF);
       
            missingFieldsReport.Visibility = emptyRF ? Visibility.Visible : Visibility.Hidden;          // Toggle

            return emptyRF;
        }

        private static void InvalidField(ref Label label, ref bool invalidField)
        {
            label.Visibility = Visibility.Visible;
            invalidField = true;
        }
        
        public static class SignUp
        {
            public static bool IsUsernameValid(string username)
            {
                return DatabaseSQL.IsUsernameAvailable(username);
            }

            public static bool PasswordsMatch(string password, string confirmPassword)
            {
                return password == confirmPassword;
            }

            public static string IsEmailValid(Page page, string email)
            {
                if (email == "") return "email cannot be empty!";

                string Email = email.Trim().ToLower();
                bool dbCheck = DatabaseSQL.IsEmailAvailable(Email);

                bool formatCheck1 = Email.Contains("@") && (Email.Contains(".com") || Email.Contains(".co.za"));
                bool formatCheck2 = !Email.Contains(" ");

                if (!dbCheck && page.Name == "signUpPage") return "This email already exist!";
                else if (!formatCheck1) return "Email is missing a '@' or .com or .co.za";
                else if (!formatCheck2) return "Email is in invalid format, cannot contain space ";

                return "";
            }

            public static bool IsFirstnameValid(string firstname)
            {
                if (firstname == "") return false;

                string illegals = "0123456789~!@#$%^&*()_+}{[]|:;/.,<>'\'|";
                char[] invalids = illegals.ToCharArray();
                string[] substrings = firstname.Split(invalids);

                return substrings.Length == 1;
            }

            public static bool IsFieldInvalid(Page page, List<object> entry_cells, List<Label> errorLabels)
            {
                bool invalidField = false;

                for (int i = 0; i < entry_cells.Count; i++)
                {
                    if (entry_cells[i] is TextBox)
                    {
                        TextBox box = (TextBox)entry_cells[i];
                        switch (box.Name)
                        {
                            case "username":
                                Label usernameErr = errorLabels.FirstOrDefault(x => x.Name == "username_validate");
                                if (!IsUsernameValid(box.Text))
                                {
                                    InvalidField(ref usernameErr, ref invalidField);
                                }
                                else
                                    usernameErr.Visibility = Visibility.Hidden;
                                break;

                            case "email":
                                Label emailErr = errorLabels.FirstOrDefault(x => x.Name == "email_validate");
                                emailErr.Content = IsEmailValid(page, box.Text);
                                if ((string)emailErr.Content != "")
                                    InvalidField(ref emailErr, ref invalidField);
                                else
                                    emailErr.Visibility = Visibility.Hidden;
                                break;

                            case "firstname":
                                Label firstnameErr = errorLabels.FirstOrDefault(x => x.Name == "firstname_validate");
                                if (!IsFirstnameValid(box.Text))
                                {
                                    InvalidField(ref firstnameErr, ref invalidField);
                                }
                                else
                                    firstnameErr.Visibility = Visibility.Hidden;
                                break;
                        }
                    }
                    else
                    {
                        PasswordBox pswdBox = (PasswordBox)entry_cells[i];
                        PasswordBox confirmBox = new PasswordBox { Password = "" };
                        for (int c = i + 1; c < entry_cells.Count; c++)
                        {
                            if (entry_cells[i] is PasswordBox)
                            {
                                confirmBox = (PasswordBox)entry_cells[c];
                            }
                        }
                        if (pswdBox.Name == "password")
                        {
                            Label pswdErr = errorLabels.FirstOrDefault(x => x.Name == "password_validate");
                            if (!PasswordsMatch(pswdBox.Password, confirmBox.Password))
                                InvalidField(ref pswdErr, ref invalidField);
                            else
                                pswdErr.Visibility = Visibility.Hidden;
                        }
                        else continue;
                    }
                }
                return invalidField;
            }

        }

        public static class LogIn
        {
            private static bool UsernameCheck(TextBox username)
            {
                /// PRE-CONDITIONS
                /// check if the username textbox has an email entered or just a name
                ///   if is email entered, check if this email is registered by checking if it still availble to be assigned
                ///   else if name, check if the name is available, if it is available then it is not in the database

                string userName = username.Text.Trim().ToLower();

                if (!userName.Contains("@") && DatabaseSQL.IsUsernameAvailable(userName))     //check if username entered is NOT an email, and if that username is still available (meaning not in the DB) thus not registerd
                    return false;
                else if (userName.Contains("@") && DatabaseSQL.IsEmailAvailable(userName))    //check if the user entered an email as username, and check if that email is still available (meaning not in the DB) thus not registered
                    return false;

                return true;
            }

            private static bool IsPasswordCorrect(TextBox username, PasswordBox password)
            {
                string userName = username.Text.Trim().ToLower();

                return DatabaseSQL.CorrectCredentials(userName, password.Password);
            }

            public static bool CredentialsValidated(List<Label> errorLabels, List<object> entryCells, Page page)
            {
                Label username_validate = errorLabels.FirstOrDefault(x => x.Name.Contains("username"));
                Label password_validate = errorLabels.FirstOrDefault(x => x.Name.Contains("password"));
                Label missing = errorLabels.FirstOrDefault(x => x.Name.Contains("missing"));

                PasswordBox password = null;
                TextBox username = null;

                // Decompose cells to pass into appropriate functions
                for(int i=0; i<entryCells.Count;i++)
                {
                    if (entryCells[i] is TextBox)
                        username = (TextBox)entryCells[i];

                    else if (entryCells[i] is PasswordBox)
                        password = (PasswordBox)entryCells[i];
                }

                // If there are empty filed
                if (RequiredFieldsEmpty(entryCells, missing)) return false;

                if (!UsernameCheck(username))
                {
                    password_validate.Visibility = Visibility.Hidden;
                    username_validate.Visibility = Visibility.Visible;
                }

                else if (!IsPasswordCorrect(username, password))
                {
                    username_validate.Visibility = Visibility.Hidden;
                    password_validate.Visibility = Visibility.Visible;
                }

                else
                {
                    username_validate.Visibility = Visibility.Hidden;
                    password_validate.Visibility = Visibility.Hidden;
                    return true;
                }

                return false;
            }
        }
    }
}
