using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
   
    public class UserUserRoleUserInfoServices
    {
        private static UserUserRoleUserInfoServices s_instance;

        public static UserUserRoleUserInfoServices Instance => s_instance ?? (s_instance = new UserUserRoleUserInfoServices());


        public UserUserRoleUserInfoServices() { }

        public User_UserRole_UserInfo FindUserUserRoleUserInfoById(Guid Id)
        {
            User_UserRole_UserInfo result = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(x => x.Id == Id).FirstOrDefault();
            return result;
        }

        public bool SaveUserInfoToDatabase(User_UserRole_UserInfo userInfo)
        {
            try
            {
                User_UserRole_UserInfo savedUserInfo = UserUserRoleUserInfoServices.Instance.FindUserUserRoleUserInfoById(userInfo.Id);

                if (savedUserInfo == null)
                {

                    DataProvider.Instance.Database.User_UserRole_UserInfo.Add(userInfo);
                }
                else
                {
                    //savedTeacher = (teacherUser.ShallowCopy() as Teacher);
                    //Reflection.CopyProperties(userInfo, savedUserInfo);
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
