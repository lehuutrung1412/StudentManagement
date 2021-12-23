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
    public class TeacherServices
    {
        private static TeacherServices s_instance;

        public static TeacherServices Instance => s_instance ?? (s_instance = new TeacherServices());

        public TeacherServices() { }

        public DbSet<Teacher> LoadTeacherList()
        {
            return DataProvider.Instance.Database.Teachers;
        }

        public Teacher FindTeacherByTeacherName(string teacherName)
        {
            Teacher teacher = DataProvider.Instance.Database.Teachers.Where(teacherItem => teacherItem.User.DisplayName == teacherName).FirstOrDefault();

            return teacher;
        }
        public Teacher FindTeacherByUserName(string userName)
        {
            Teacher teacher = DataProvider.Instance.Database.Teachers.Where(userItem => userItem.User.Username == userName).FirstOrDefault();

            return teacher;
        }
        /// <summary>
        /// Convert TeacherCard To Teacher
        /// </summary>
        /// <param name="teacherCard"></param>
        /// <returns>Teacher</returns>
        /*public Teacher ConvertTeacherCardToTeacher(TeacherCard teacherCard)
        {
            Teacher teacher = new Teacher()
            {
                Id = teacherCard.Id,
                DisplayName = teacherCard.DisplayName
            };

            return teacher;
        }*/

        /// <summary>
        /// Convert Teacher To Teacher Card
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns>TeacherCard</returns>
        /*public TeacherCard ConvertTeacherToTeacherCard(Teacher teacher)
        {
            TeacherCard teacherCard = new TeacherCard(teacher.Id, teacher.DisplayName, new DateTime(2015, 12, 31), 100, "test");

            return teacherCard;
        }*/

        /// <summary>
        /// Find Teacher By Teacher Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Teacher || null</returns>
        public Teacher FindTeacherByTeacherId(Guid id)
        {
            Teacher teacher = DataProvider.Instance.Database.Teachers.Where(teacherItem => teacherItem.Id == id).FirstOrDefault();

            return teacher;
        }

        /// <summary>
        /// Save Teacher To Database
        /// </summary>
        /// <param name="teacher"></param>
        public bool SaveTeacherToDatabase(Teacher teacher)
        {
            try
            {
                Teacher savedUser = TeacherServices.Instance.FindTeacherByTeacherId(teacher.Id);

                if (savedUser == null)
                {
                    
                    DataProvider.Instance.Database.Teachers.Add(teacher);
                }
                else
                {
                    //savedTeacher = (teacherUser.ShallowCopy() as Teacher);
                    Reflection.CopyProperties(teacher, savedUser);
                }
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save Teacher Card To Database
        /// </summary>
        /// <param name="teacherCard"></param>
        /*public void SaveTeacherCardToDatabase(TeacherCard teacherCard)
        {
            Teacher teacher = ConvertTeacherCardToTeacher(teacherCard);

            SaveTeacherToDatabase(teacher);
        }*/

        /// <summary>
        /// Remove Teacher From Database
        /// </summary>
        /// <param name="teacher"></param>
        public void RemoveTeacherFromDatabase(Teacher teacher)
        {
            Teacher savedTeacher = FindTeacherByTeacherId(teacher.Id);

            DataProvider.Instance.Database.Teachers.Remove(savedTeacher);

            DataProvider.Instance.Database.SaveChanges();
        }

        public Teacher GetTeacherbyUser(User user)
        {
            return DataProvider.Instance.Database.Teachers.FirstOrDefault(teacher => teacher.IdUsers == user.Id);
        }

        /// <summary>
        /// Remove TeacherCard From Database
        /// </summary>
        /// <param name="teacherCard"></param>
        /*public void RemoveTeacherCardFromDatabase(TeacherCard teacherCard)
        {
            Teacher teacher = ConvertTeacherCardToTeacher(teacherCard);

            RemoveTeacherFromDatabase(teacher);
        }*/
    }
}
