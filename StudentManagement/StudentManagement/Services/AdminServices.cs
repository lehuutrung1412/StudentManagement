using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class AdminServices
    {
        private static AdminServices s_instance;

        public static AdminServices Instance => s_instance ?? (s_instance = new AdminServices());

        public AdminServices() { }

        public Admin GetAdminByUser(User user)
        {
            return DataProvider.Instance.Database.Admins.FirstOrDefault(admin=>admin.IdUsers == user.Id);
        }
    }
}
