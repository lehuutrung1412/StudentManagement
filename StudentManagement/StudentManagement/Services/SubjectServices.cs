using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class SubjectServices
    {
        private static SubjectServices s_instance;

        public static SubjectServices Instance => s_instance ?? (s_instance = new SubjectServices());

        public SubjectServices() { }

        public DbSet<Subject> LoadSubjectList()
        {
            return DataProvider.Instance.Database.Subjects;
        }

        public Subject FindSubjectBySubjectName(string subjectName)
        {
            Subject subject = DataProvider.Instance.Database.Subjects.Where(subjectItem => subjectItem.DisplayName == subjectName).FirstOrDefault();

            return subject;
        }
        /// <summary>
        /// Convert SubjectClassCard To Subject
        /// </summary>
        /// <param name="subjectClassCard"></param>
        /// <returns>Subject</returns>
        /*public Subject ConvertSubjectClassCardToSubject(SubjectClassCard subjectClassCard)
        {
            Subject subject = new Subject()
            {
                Id = subjectClassCard.Id,
                DisplayName = subjectClassCard.DisplayName
            };

            return subject;
        }*/

        /// <summary>
        /// Convert Subject To Subject Card
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>SubjectClassCard</returns>
        /*public SubjectClassCard ConvertSubjectToSubjectClassCard(Subject subject)
        {
            SubjectClassCard subjectClassCard = new SubjectClassCard(subject.Id, subject.DisplayName, new DateTime(2015, 12, 31), 100, "test");

            return subjectClassCard;
        }*/

        /// <summary>
        /// Find Subject By Subject Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Subject || null</returns>
        public Subject FindSubjectBySubjectId(Guid id)
        {
            Subject subject = DataProvider.Instance.Database.Subjects.Where(subjectItem => subjectItem.Id == id).FirstOrDefault();

            return subject;
        }

        /// <summary>
        /// Save Subject To Database
        /// </summary>
        /// <param name="subject"></param>
        public bool SaveSubjectToDatabase(Subject subject)
        {
            try
            {
                DataProvider.Instance.Database.Subjects.AddOrUpdate(subject);
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Save Subject Card To Database
        /// </summary>
        /// <param name="subjectClassCard"></param>
        /*public void SaveSubjectClassCardToDatabase(SubjectClassCard subjectClassCard)
        {
            Subject subject = ConvertSubjectClassCardToSubject(subjectClassCard);

            SaveSubjectToDatabase(subject);
        }*/

        /// <summary>
        /// Remove Subject From Database
        /// </summary>
        /// <param name="subject"></param>
        public bool RemoveSubjectFromDatabase(Subject subject)
        {
            try
            {
                Subject savedSubject = FindSubjectBySubjectId(subject.Id);

                // soft delete
                savedSubject.IsDeleted = true;

                //DataProvider.Instance.Database.Subjects.Remove(savedSubject);

                DataProvider.Instance.Database.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove SubjectClassCard From Database
        /// </summary>
        /// <param name="subjectClassCard"></param>
        /*public void RemoveSubjectClassCardFromDatabase(SubjectClassCard subjectClassCard)
        {
            Subject subject = ConvertSubjectClassCardToSubject(subjectClassCard);

            RemoveSubjectFromDatabase(subject);
        }*/

        public SubjectCard ConvertSubjectToSubjectCard(Subject subject)
        {
            SubjectCard subjectCard = new SubjectCard(subject.Id, subject.DisplayName, subject.Credit, subject.Code, subject.Describe);

            return subjectCard;
        }

        public Subject ConvertSubjectCardToSubject(SubjectCard subjectCard)
        {
            Subject subject = new Subject()
            {
                Id = subjectCard.Id,
                DisplayName = subjectCard.DisplayName,
                Credit = subjectCard.Credit,
                Code = subjectCard.Code,
                Describe = subjectCard.Describe,
                IsDeleted = subjectCard.IsDeleted
            };

            return subject;
        }

        public bool RemoveSubjectCardFromDatabase(SubjectCard subjectCard)
        {
            Subject subject = ConvertSubjectCardToSubject(subjectCard);

            return RemoveSubjectFromDatabase(subject);
        }

        public bool SaveSubjectCardToDatabase(SubjectCard subjectCard)
        {
            try
            {
                Subject subject = ConvertSubjectCardToSubject(subjectCard);

                SaveSubjectToDatabase(subject);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
