using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class FacultyServices
    {
        private static FacultyServices s_instance;

        public static FacultyServices Instance => s_instance ?? (s_instance = new FacultyServices());

        public FacultyServices() { }

        //public Faculty ConvertFacultyCardToFaculty(FacultyCard facultyCard)
        //{
        //    Faculty faculty = new Faculty()
        //    {
        //        Id = facultyCard.Id,
        //        DisplayName = 
        //    }
        //}
    }
}
