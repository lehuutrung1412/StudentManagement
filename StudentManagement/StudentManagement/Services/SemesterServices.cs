using StudentManagement.Models;
using StudentManagement.Utils;
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

        public Semester FindSemesterBySemesterId(Guid id)
        {
            Semester a = DataProvider.Instance.Database.Semesters.Where(semesterItem=>semesterItem.Id == id).FirstOrDefault();
            return a;
        }
        public ObservableCollection<Semester> LoadListSemester()
        {
            var a = DataProvider.Instance.Database.Semesters.OrderBy(y => y.DisplayName).OrderBy(x => x.Batch).ToList();
            return new ObservableCollection<Semester>(a);
        }

        public ObservableCollection<Semester> LoadListSemestersByBatch(string batch)
        {
            var a = DataProvider.Instance.Database.Semesters.Where(semesterItem=>semesterItem.Batch == batch).ToList();
            return new ObservableCollection<Semester>(a);
        }

        public bool SaveSemesterToDatabase(Semester semester)
        {
            try
            {
                Semester savedSemester = FindSemesterBySemesterId(semester.Id);

                if (savedSemester == null)
                {
                    DataProvider.Instance.Database.Semesters.Add(semester);
                }
                else
                {
                    //savedFaculty = (faculty.ShallowCopy() as Faculty);
                    Reflection.CopyProperties(semester, savedSemester);
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
