using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Folder_Locker.Services
{
    public static class Security
    {
        
        private static void DummyFile(string folderPath)
        {
            TextWriter textWriter = File.CreateText(folderPath + "\\FolderLock.bat");

            #region Writing File
            {
                textWriter.WriteLine("This is .bat file has been used to hide files or folders,");
                textWriter.WriteLine(" Use the App \"Folder App to unlock them :) \" ");
            }
            #endregion

            textWriter.Close();
        }

        private static void CreateBatLockFile(string folderPath)
        {
            TextWriter textWriter = File.CreateText(folderPath + "\\FolderLock.bat");

            #region Writing File
            {
                textWriter.WriteLine("cls");
                textWriter.WriteLine("@ECHO OFF");
                textWriter.WriteLine("title Folder Locker");
                textWriter.WriteLine("@ECHO OFF");
                textWriter.WriteLine();

                textWriter.WriteLine("if EXIST \"Control Panel.{ 21EC2020 - 3AEA - 1069 - A2DD - 08002B30309D}\" goto UNLOCK");
                textWriter.WriteLine("if NOT EXIST LOCKER goto MDLOCKER");
                textWriter.WriteLine();

                textWriter.WriteLine(":CONFIRM");
                textWriter.WriteLine("echo Are you sure u want to Lock the folder(Y/N)");
                textWriter.WriteLine("set/p \"choice=>\"");
                textWriter.WriteLine("if %choice%==Y goto LOCK");
                textWriter.WriteLine("if %choice%==y goto LOCK");
                textWriter.WriteLine("if %choice%==n goto END");
                textWriter.WriteLine("if %choice%==N goto END");
                textWriter.WriteLine("echo Invalid choice.");
                textWriter.WriteLine("goto CONFIRM");
                textWriter.WriteLine();

                textWriter.WriteLine(":LOCK");
                textWriter.WriteLine("ren LOCKER \"Control Panel.{ 21EC2020 - 3AEA - 1069 - A2DD - 08002B30309D}\" ");
                textWriter.WriteLine("attrib +h +s \"Control Panel.{ 21EC2020 - 3AEA - 1069 - A2DD - 08002B30309D}\" ");
                textWriter.WriteLine("echo Folder locked");
                textWriter.WriteLine("goto End");
                textWriter.WriteLine();

                textWriter.WriteLine(":UNLOCK");
                textWriter.WriteLine("echo Enter password to Unlock folder");
                textWriter.WriteLine("set/p \"pass=>\"");
                textWriter.WriteLine("if NOT %pass%==1234 goto FAIL");
                textWriter.WriteLine("attrib -h -s \"Control Panel.{ 21EC2020 - 3AEA - 1069 - A2DD - 08002B30309D}\"");
                textWriter.WriteLine("ren \"Control Panel.{ 21EC2020 - 3AEA - 1069 - A2DD - 08002B30309D}\" LOCKER");
                textWriter.WriteLine("echo Folder Unlocked successfully");
                textWriter.WriteLine("goto End");
                textWriter.WriteLine();

                textWriter.WriteLine(":FAIL");
                textWriter.WriteLine("echo Invalid password");
                textWriter.WriteLine("goto end");
                textWriter.WriteLine();

                textWriter.WriteLine(":MDLOCKER");
                textWriter.WriteLine("md LOCKER");
                textWriter.WriteLine("echo LOCKER created successfully");
                textWriter.WriteLine("goto End");
                textWriter.WriteLine();

                textWriter.WriteLine(":End");
            }
            #endregion

            textWriter.Close();
        }
        
        private static ProcessStartInfo ProcessInfo(string folderPath)
        {
            CreateBatLockFile(folderPath);                                   // Update .batFile to be able to unlock when executed

            string batFilePath = "\"" + folderPath + "\\FolderLock.bat" + "\"";            // .bat file path to execute            

            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + batFilePath);   // run windows command line with the input as .bat file path

            // Process Properties
            processInfo.CreateNoWindow = true;                                      // Do not display the cmd line
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = folderPath;                       // Set directory to be where the .bat file is 
            processInfo.RedirectStandardError = true;                               // Error reporting
            processInfo.RedirectStandardOutput = true;                              // Display output
            processInfo.RedirectStandardInput = true;                               // Supply inputs

            return processInfo;
        }

        public static void CreateLockerFolder(string folderPath)
        {
            Process process = Process.Start(ProcessInfo(folderPath));         // Create and Start a windows process with the above defined properties
            process.StandardInput.Write("N");                                            // Dummy input

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            //MessageBox.Show(string.Format("output : {0}\nerror: {1}", output, error));

            process.Close();

            DummyFile(folderPath);
        }
        
        public static void LockFolder(string folderPath)
        {
            Process process = Process.Start(ProcessInfo(folderPath));               // Create and Start a windows process with the above defined properties

            process.StandardInput.Write("Y");                                       // Supply input (AKA -> Lock? = "Yes" )

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

           // MessageBox.Show(string.Format("out : {0}\nerr: {1}", output, error));

            process.Close();

            DummyFile(folderPath);
        }

        public static void UnlockFolder(string folderPath)
        {
            Process process = Process.Start(ProcessInfo(folderPath));   // Create and Start a windows process with the above defined properties

            process.StandardInput.Write("1234");                        // Supply input (AKA -> Password )

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

           // MessageBox.Show(string.Format("out : {0}\nerr: {1}", output, error));

            process.Close();                                        // Close the Process

            DummyFile(folderPath);
        }

        private static void AddOption_ContextMenu()
        {
            RegistryKey _key = Registry.ClassesRoot.OpenSubKey("Directory\\Background\\Shell", true);
            RegistryKey newkey = _key.CreateSubKey("Your Application");
            RegistryKey subNewkey = newkey.CreateSubKey("Command");
            subNewkey.SetValue("", "C:\\FolderLocker.exe");
            subNewkey.Close();
            newkey.Close();
            _key.Close();
        }

        private static void RemoveOption_ContextMenu()
        {
            RegistryKey _key = Registry.ClassesRoot.OpenSubKey("Directory\\Background\\Shell\\", true);
            _key.DeleteSubKey("Your Application");
            _key.Close();
        }

        public static void AddOption_ContextMenuFolder()
        {
            RegistryKey _key = Registry.ClassesRoot.OpenSubKey("Folder\\Shell", true);
            RegistryKey newkey = _key.CreateSubKey("Lock with Folder Locker");
            RegistryKey subNewkey = newkey.CreateSubKey("Command");
            subNewkey.SetValue("", "C:\\FolderLocker.exe");
            subNewkey.Close();
            newkey.Close();
            _key.Close();
        }

        private static void RemoveOption_ContextMenuFolder()
        {
            RegistryKey _key = Registry.ClassesRoot.OpenSubKey("Folder\\Shell\\", true);
            _key.DeleteSubKey("Lock with Folder Locker");
            _key.Close();
        }


        private static void Previledges()
        {
            try
            {
                string DirectoryName = Globals.GetCurrentFolder().FolderPath;

                MessageBox.Show("Adding access control entry for " + DirectoryName);



                //Add the access control entry to the directory.
                AddDirectorySecurity(DirectoryName, @"MYDOMAIN\MyAccount", FileSystemRights.FullControl, AccessControlType.Allow);

                MessageBox.Show("Removing access control entry from " + DirectoryName);

                //Remove the access control entry from the directory.
                RemoveDirectorySecurity(DirectoryName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AccessControlType.Allow);

                MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        // Adds an ACL entry on the specified directory for the specified account.
        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {

            IdentityReference sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            DirectorySecurity dSecurity = Directory.GetAccessControl(FileName);

            FileSystemAccessRule full_access_rule = new FileSystemAccessRule(sid,
                            FileSystemRights.FullControl, InheritanceFlags.ContainerInherit |
                             InheritanceFlags.ObjectInherit, PropagationFlags.None,
                             AccessControlType.Allow);

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(full_access_rule);

            // Set the new access settings.
            Directory.SetAccessControl(FileName, dSecurity);

        }

        // Removes an ACL entry on the specified directory for the specified account.
        public static void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(sid, Rights, ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);
            MessageBox.Show("" + dSecurity.GetType());

        }

    }
}
