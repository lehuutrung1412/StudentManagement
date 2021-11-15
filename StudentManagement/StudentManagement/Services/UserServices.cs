using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class UserServices
    {
        private static UserServices s_instance;

        public static UserServices Instance => s_instance ?? (s_instance = new UserServices());

        public UserServices() { }

        public User GetUserInfo()
        {
            User a = DataProvider.Instance.Database.Users.FirstOrDefault();
            return a;
        }
    }
}
