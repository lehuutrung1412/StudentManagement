using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class SemesterServices
    {
        private static SemesterServices s_instance;

        public static SemesterServices Instance => s_instance ?? (s_instance = new SemesterServices());

        public SemesterServices() { }

        public Semester GetSemester()
        {
            Semester a = DataProvider.Instance.Database.Semesters.FirstOrDefault();
            return a;
        }
        public ObservableCollection<Semester> GetSemesters()
        {
            var a = DataProvider.Instance.Database.Semesters;
            return new ObservableCollection<Semester>(a);
        }
    }
}
