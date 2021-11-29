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
        /// Convert SubjectCard To Subject
        /// </summary>
        /// <param name="subjectCard"></param>
        /// <returns>Subject</returns>
        /*public Subject ConvertSubjectCardToSubject(SubjectCard subjectCard)
        {
            Subject subject = new Subject()
            {
                Id = subjectCard.Id,
                DisplayName = subjectCard.DisplayName
            };

            return subject;
        }*/

        /// <summary>
        /// Convert Subject To Subject Card
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>SubjectCard</returns>
        /*public SubjectCard ConvertSubjectToSubjectCard(Subject subject)
        {
            SubjectCard subjectCard = new SubjectCard(subject.Id, subject.DisplayName, new DateTime(2015, 12, 31), 100, "test");

            return subjectCard;
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
        /// <param name="subjectCard"></param>
        /*public void SaveSubjectCardToDatabase(SubjectCard subjectCard)
        {
            Subject subject = ConvertSubjectCardToSubject(subjectCard);

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
        /// Remove SubjectCard From Database
        /// </summary>
        /// <param name="subjectCard"></param>
        /*public void RemoveSubjectCardFromDatabase(SubjectCard subjectCard)
        {
            Subject subject = ConvertSubjectCardToSubject(subjectCard);

            RemoveSubjectFromDatabase(subject);
        }*/
    }
}
