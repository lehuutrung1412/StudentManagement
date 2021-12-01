using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class DatabaseImageTableServices
    {
        private static DatabaseImageTableServices s_instance;

        public static DatabaseImageTableServices Instance => s_instance ?? (s_instance = new DatabaseImageTableServices());

        public DatabaseImageTableServices() { }

        public DatabaseImageTable GetDatabaseImageTable()
        {
            return DataProvider.Instance.Database.DatabaseImageTables.FirstOrDefault();
        }
    }
}
