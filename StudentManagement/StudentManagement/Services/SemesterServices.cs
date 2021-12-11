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

        //CourseRegistryStatus:
        //0-Tạm đóng đăng ký
        //1-Đang mở đăng ký,
        //2-Kết thúc đợt đăng ký học phần, đưa sinh viên vào lớp học và update TKB SV
        public Semester FindSemesterBySemesterId(Guid id)
        {
            Semester a = DataProvider.Instance.Database.Semesters.Where(semesterItem=>semesterItem.Id == id).FirstOrDefault();
            return a;
        }

        public Semester GetFirstOpenningRegisterSemester()
        {
            var listSemesterDesc = DataProvider.Instance.Database.Semesters.OrderByDescending(y => y.DisplayName).OrderByDescending(x => x.Batch);
            Semester a = listSemesterDesc.Where(semesterItem => semesterItem.CourseRegisterStatus == 1).FirstOrDefault();
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

        public ObservableCollection<Semester> LoadListSemestersByStudentIdAndSemesterStatuses(Guid idStudent, bool[] semesterStatus)
        {
            var listSemester = new List<Semester>();
            var listCourseRegister = DataProvider.Instance.Database.CourseRegisters.Where(register => register.IdStudent == idStudent).ToList();
            for(int i = 0; i<semesterStatus.Length; i++)
            {
                if (!semesterStatus[i])
                    listCourseRegister = listCourseRegister.Where(register => register.Semester.CourseRegisterStatus != i).ToList();
            }
            foreach(CourseRegister register in listCourseRegister)
            {
                listSemester.Add(register.Semester);
            }
            return new ObservableCollection<Semester>(listSemester.Distinct().ToList());
        }

        public ObservableCollection<Semester> LoadListSemestersByTeacherAndSemesterStatuses(Teacher teacher, bool[] semesterStatus)
        {
            var listSemester = new List<Semester>();
            var listSubjectClass = DataProvider.Instance.Database.SubjectClasses.Where(subjectClass => subjectClass.Teachers.FirstOrDefault().Id == teacher.Id).ToList();
            for (int i = 0; i < semesterStatus.Length; i++)
            {
                if (!semesterStatus[i])
                    listSubjectClass = listSubjectClass.Where(subjectClass => subjectClass.Semester.CourseRegisterStatus != i).ToList();
            }
            foreach (SubjectClass subjectClass in listSubjectClass)
            {
                listSemester.Add(subjectClass.Semester);
            }
            return new ObservableCollection<Semester>(listSemester.Distinct().ToList());
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
