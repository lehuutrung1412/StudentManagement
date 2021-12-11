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

        #region Convert

        public StudentGrid ConvertStudentToStudentGrid(Student student, int number = 0)
        {
            return new StudentGrid()
            {
                Id = student.Id,
                Number = number,
                DisplayName = student.User.DisplayName,
                Email = student.User.Email,
                Faculty = student.Faculty.DisplayName,
                TrainingForm = student.TrainingForm.DisplayName,
                Username = student.User.Username,
                Status = student.User.Online == true ? "Trực tuyến" : "Ngoại tuyến"
            };
        }

        #endregion


        public Student GetFirstStudent()
        {
            return DataProvider.Instance.Database.Students.FirstOrDefault();
        }

        public DbSet<Student> LoadStudentList()
        {
            return DataProvider.Instance.Database.Students;
        }

        public Student FindStudentByStudentId(Guid id)
        {
            Student a = DataProvider.Instance.Database.Students.Where(studentItem => studentItem.Id == id).FirstOrDefault();
            return a;
        }

        public bool SaveStudentToDatabase(Student student)
        {
            try
            {
                Student savedStudent = FindStudentByStudentId(student.Id);

                if (savedStudent == null)
                {
                    DataProvider.Instance.Database.Students.Add(student);
                }
                else
                {
                    //savedFaculty = (faculty.ShallowCopy() as Faculty);
                    Reflection.CopyProperties(student, savedStudent);
                }
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Student GetStudentbyUser(User user)
        {
            return DataProvider.Instance.Database.Students.FirstOrDefault(student => student.IdUsers == user.Id);
        }


    }
}
