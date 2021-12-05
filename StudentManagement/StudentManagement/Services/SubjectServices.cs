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
        public void SaveSubjectToDatabase(Subject subject)
        {
            Subject savedSubject = FindSubjectBySubjectId(subject.Id);

            if (savedSubject == null)
            {
                DataProvider.Instance.Database.Subjects.Add(subject);
            }
            else
            {
                //savedSubject = (subject.ShallowCopy() as Subject);
                Reflection.CopyProperties(subject, savedSubject);
            }
            DataProvider.Instance.Database.SaveChanges();
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
        public void RemoveSubjectFromDatabase(Subject subject)
        {
            Subject savedSubject = FindSubjectBySubjectId(subject.Id);

            DataProvider.Instance.Database.Subjects.Remove(savedSubject);

            DataProvider.Instance.Database.SaveChanges();
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
    }
}
