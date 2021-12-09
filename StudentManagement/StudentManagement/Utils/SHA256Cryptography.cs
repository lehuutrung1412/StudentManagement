using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Utils
{
    public class SHA256Cryptography
    {
        private static SHA256Cryptography s_instance;

        public static SHA256Cryptography Instance
        {
            get => s_instance ?? (s_instance = new SHA256Cryptography());

            private set => s_instance = value;
        }

        public SHA256Cryptography() { }

        public string EncryptString(string str="")
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
