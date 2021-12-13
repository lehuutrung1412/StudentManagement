using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class Account
    {
        private string _userName;

        // Hash style
        private string _passWord;

        public string UserName { get => _userName; set => _userName = value; }
        public string PassWord { get => _passWord; set => _passWord = value; }

        public Account(string userName, string passWord)
        {
            this.UserName = userName;
            this.PassWord = passWord;
        }
    }
}
