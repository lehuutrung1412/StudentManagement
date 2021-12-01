using StudentManagement.Components;
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
    public class SubjectClassServices
    {
        private static SubjectClassServices s_instance;

        public static SubjectClassServices Instance => s_instance ?? (s_instance = new SubjectClassServices());

        public SubjectClassServices() { }

        public DbSet<SubjectClass> LoadSubjectClassList()
        {
            return DataProvider.Instance.Database.SubjectClasses;
        }

        public ObservableCollection<SubjectClass> LoadSubjectClassListBySemesterId(Guid id)
        {
            return new ObservableCollection<SubjectClass>(DataProvider.Instance.Database.SubjectClasses.Where(subjectClass => subjectClass.Semester.Id == id).ToList());
        }
        
        /*public SubjectClass ConvertSubjectCardToSubjectClass(SubjectCard subjectCard)
        {
            SubjectClass subjectClass = new SubjectClass()
            {
                Id = subjectCard.
                DisplayName = subjectCard.DisplayName
            };

            return faculty;
        }*/

        /*public SubjectCard ConvertSubjectClassToSubjectCard(SubjectClass subjectClass)
        {
            SubjectCard subjectCard = new SubjectCard(subjectClass.NumberStudent);

            return facultyCard;
        }*/

        /// <summary>
        /// Find SubjectClass By SubjectClass Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SubjectClass || null</returns>
        public SubjectClass FindSubjectClassBySubjectClassId(Guid id)
        {
            SubjectClass subjectClass = DataProvider.Instance.Database.SubjectClasses.Where(subjectClassItem => subjectClassItem.Id == id).FirstOrDefault();

            return subjectClass;
        }

        /// <summary>
        /// Save SubjectClass To Database
        /// </summary>
        /// <param name="subjectClass"></param>
        public void SaveSubjectClassToDatabase(SubjectClass subjectClass)
        {
            SubjectClass savedSubjectClass = FindSubjectClassBySubjectClassId(subjectClass.Id);

            if (savedSubjectClass == null)
            {
                DataProvider.Instance.Database.SubjectClasses.Add(subjectClass);
            }
            else
            {
                //savedSubjectClass = (subjectClass.ShallowCopy() as SubjectClass);
                Reflection.CopyProperties(subjectClass, savedSubjectClass);
            }
            DataProvider.Instance.Database.SaveChanges();
        }

        /// <summary>
        /// Save SubjectClass Card To Database
        /// </summary>
        /// <param name="subjectClassCard"></param>
        /*public void SaveSubjectClassCardToDatabase(SubjectClassCard subjectClassCard)
        {
            SubjectClass subjectClass = ConvertSubjectClassCardToSubjectClass(subjectClassCard);

            SaveSubjectClassToDatabase(subjectClass);
        }*/

        /// <summary>
        /// Remove SubjectClass From Database
        /// </summary>
        /// <param name="subjectClass"></param>
        public void RemoveSubjectClassFromDatabase(SubjectClass subjectClass)
        {
            SubjectClass savedSubjectClass = FindSubjectClassBySubjectClassId(subjectClass.Id);

            DataProvider.Instance.Database.SubjectClasses.Remove(savedSubjectClass);

            DataProvider.Instance.Database.SaveChanges();
        }

        public void RemoveSubjectClassFromDatabaseBySubjectClassId(Guid id)
        {
            SubjectClass savedSubjectClass = FindSubjectClassBySubjectClassId(id);

            DataProvider.Instance.Database.SubjectClasses.Remove(savedSubjectClass);

            DataProvider.Instance.Database.SaveChanges();
        }
        /// <summary>
        /// Remove SubjectClassCard From Database
        /// </summary>
        /// <param name="subjectClassCard"></param>
        /*public void RemoveSubjectClassCardFromDatabase(SubjectClassCard subjectClassCard)
        {
            SubjectClass subjectClass = ConvertSubjectClassCardToSubjectClass(subjectClassCard);

            RemoveSubjectClassFromDatabase(subjectClass);
        }*/

    }
}
