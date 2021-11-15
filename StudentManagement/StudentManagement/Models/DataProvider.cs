using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class DataProvider
    {
        private static DataProvider s_instance;

        public static DataProvider Instance => s_instance ?? (s_instance = new DataProvider());

        private DataProvider()
        {
            Database = new StudentManagementEntities();
        }

        public StudentManagementEntities Database { get; set; }
    }
}
