using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Folder_Locker.Model
{
    public class Person
    {
        public Image ProfilePicture { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string Password { get; set; }
    }
}
