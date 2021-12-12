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
        private string _passWordHash;

        public string UserName { get => _userName; set => _userName = value; }
        public string PassWordHash { get => _passWordHash; set => _passWordHash = value; }

        public Account(string userName, string passWordHash)
        {
            this.UserName = userName;
            this.PassWordHash = passWordHash;
        }
    }
}
