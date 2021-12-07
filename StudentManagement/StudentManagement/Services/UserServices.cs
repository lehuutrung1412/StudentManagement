using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        //public string GetFacultyById(Guid id)
        //{
        //    var user = GetUserById(id);
        //    return user.Faculty.DisplayName;
        //}

        public bool CheckAdminByIdUser(Guid id)
        {
            return DataProvider.Instance.Database.Users.FirstOrDefault(user => user.Id == id).UserRole.Role.Contains("Admin");
        }

        public User FindUserbyUserId(Guid id)
        {
            User a = DataProvider.Instance.Database.Users.Where(userItem => userItem.Id == id).FirstOrDefault();
            return a;
        }

        public bool SaveUserToDatabase(User user)
        {
            try
            {
                User savedUser = FindUserbyUserId(user.Id);

                if (savedUser == null)
                {
                    DataProvider.Instance.Database.Users.AddOrUpdate(user);
                }
                else
                {
                    //savedFaculty = (faculty.ShallowCopy() as Faculty);
                    //Reflection.CopyProperties(user, savedUser);
                }
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
