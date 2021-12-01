using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class FileUploader
    {
        private static FileUploader s_instance;

        public static FileUploader Instance => s_instance ?? (s_instance = new FileUploader());

        public string Upload(string file)
        {
            return null;
        }

        public string GetServer()
        {
            return null;
        }
    }
}
