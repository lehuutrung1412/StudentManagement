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
        public User GetUserById(Guid id)
        {
            User a = DataProvider.Instance.Database.Users.Where(user=>user.Id==id).FirstOrDefault();
            return a;
        }
        public string GetDisplayNameById(Guid id)
        {
            var user = GetUserById(id);
            return user.DisplayName;
        }
        public List<User> GetUserByGmail(string email)
        {
            return DataProvider.Instance.Database.Users.Where(user => user.Email.Equals(email)).ToList();
        }
        public bool CheckAdminByIdUser(Guid id)
        {
            return DataProvider.Instance.Database.Users.FirstOrDefault(user => user.Id == id).UserRole.Role.Contains("Admin");
        }
        public bool ChangePassWord(string passWord, string gmail)
        {
            var user = GetUserByGmail(gmail);
            if (user.Count == 0)
                return false;
            user.FirstOrDefault().Password = passWord;
            DataProvider.Instance.Database.SaveChanges();
            return true;
        }  
        public bool CheckLogin(string userName, string passWord)
        {
            var user = DataProvider.Instance.Database.Users.Where(tmpUser=>tmpUser.Username == userName && tmpUser.Password==passWord).ToList();
            if(user.Count()>0)
                return true;
            return false;
        }
    }
}
