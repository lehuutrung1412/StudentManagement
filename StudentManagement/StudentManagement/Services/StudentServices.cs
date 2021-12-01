using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class StudentServices
    {
        private static StudentServices s_instance;

        public static StudentServices Instance => s_instance ?? (s_instance = new StudentServices());

        public StudentServices() { }

        public Student GetFirstStudent()
        {
            return DataProvider.Instance.Database.Students.FirstOrDefault();
        }
        

    }
}
