using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class LoginServices
    {
        private static LoginServices s_instance;

        public static LoginServices Instance => s_instance ?? (s_instance = new LoginServices());

        private static User s_currentUser;
        public static User CurrentUser { get => s_currentUser; set => s_currentUser = value; }


        public StudentManagementEntities db = DataProvider.Instance.Database;


        public LoginServices() { }

        public bool IsUserAuthentic(string username, string password)
        {
            //string passEncode = MD5Hash(Base64Encode(password));
            string passEncode = password;

            int accCount = db.Users.Where(user => user.Username == username && user.Password == passEncode).Count();

            if (accCount > 0)
            {
                return true;
            }
            return false;
        }

        public void Login(string username)
        {
            User user = UserServices.Instance.FindUserByUsername(username);

            CurrentUser = user;
        }

        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
