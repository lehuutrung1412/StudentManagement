using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
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

        public List<SubjectClass> MinimizeSubjectClassListBySemesterStatus(List<SubjectClass> listSubjectClass, bool[] semesterStatus)
        {
            for (int i = 0; i < semesterStatus.Length; i++)
            {
                if (!semesterStatus[i])
                    listSubjectClass = listSubjectClass.Where(subjectClass => subjectClass.Semester.CourseRegisterStatus != i).ToList();
            }
            return listSubjectClass;
        }

        public List<SubjectClass> LoadSubjectClassListBySemesterId(Guid id)
        {
            return DataProvider.Instance.Database.SubjectClasses.Where(subjectClass => subjectClass.Semester.Id == id).ToList();
        }

        /*public SubjectClass ConvertSubjectClassCardToSubjectClass(SubjectClassCard subjectClassCard)
        {
            SubjectClass subjectClass = new SubjectClass()
            {
                Id = subjectClassCard.
                DisplayName = subjectClassCard.DisplayName
            };

            return faculty;
        }*/

        /*public SubjectClassCard ConvertSubjectClassToSubjectClassCard(SubjectClass subjectClass)
        {
            SubjectClassCard subjectClassCard = new SubjectClassCard(subjectClass.NumberStudent);

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
        public bool SaveSubjectClassToDatabase(SubjectClass subjectClass)
        {
            try
            {
                DataProvider.Instance.Database.SubjectClasses.AddOrUpdate(subjectClass);
                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    MyMessageBox.Show($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        MyMessageBox.Show($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                return false;
            }
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
        public bool RemoveSubjectClassFromDatabase(SubjectClass subjectClass)
        {

            try
            {
                SubjectClass savedSubjectClass = FindSubjectClassBySubjectClassId(subjectClass.Id);

                if (savedSubjectClass.CourseRegisters.Count() > 0)

                    return false;

                DataProvider.Instance.Database.SubjectClasses.Remove(savedSubjectClass);

                DataProvider.Instance.Database.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveSubjectClassFromDatabaseBySubjectClassId(Guid id)
        {
            try
            {
                SubjectClass savedSubjectClass = FindSubjectClassBySubjectClassId(id);

                DataProvider.Instance.Database.SubjectClasses.Remove(savedSubjectClass);

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
            SubjectClass subjectClass = ConvertSubjectClassCardToSubjectClass(subjectClassCard);

            RemoveSubjectClassFromDatabase(subjectClass);
        }*/
        public bool IsValidPeriod(string period)
        {
            // Max period of subject class is 5
            if (period?.Length <= 5)
            {
                try
                {
                    if (period.TrimStart('0') != period)
                    {
                        return false;
                    }

                    int intPeriod = Convert.ToInt32(period);

                    int lastDigit = intPeriod % 10;
                    intPeriod /= 10;

                    if (lastDigit == 0)
                    {
                        lastDigit = 10;
                    }

                    while (intPeriod != 0)
                    {
                        int extractDigit = intPeriod % 10;

                        if (extractDigit == 0)
                        {
                            extractDigit = 10;
                        }

                        if (extractDigit != lastDigit - 1)
                        {
                            return false;
                        }

                        lastDigit = extractDigit;
                        intPeriod /= 10;
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public SubjectClassCard ConvertSubjectClassToSubjectClassCard(SubjectClass subjectClass)
        {
            var DayOfWeeks = new ObservableCollection<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };

            SubjectClassCard subjectClassCard = new SubjectClassCard()
            {
                Id = subjectClass.Id,
                Code = subjectClass.Code,
                StartDate = subjectClass.StartDate,
                EndDate = subjectClass.EndDate,
                Period = subjectClass.Period,
                MaxNumberOfStudents = subjectClass.MaxNumberOfStudents,
                NumberOfStudents = subjectClass.NumberOfStudents,
                //get main teacher of the class
                SelectedTeacher = subjectClass.Teachers.FirstOrDefault(),
                SelectedSubject = subjectClass.Subject,
                SelectedTrainingForm = subjectClass.TrainingForm,
                SelectedSemester = subjectClass.Semester,
                SelectedDay = DayOfWeeks[(int)subjectClass.WeekDay],
            };
            return subjectClassCard;
        }

        public SubjectClass ConvertSubjectClassCardToSubjectClass(SubjectClassCard subjectClassCard)
        {
            SubjectClass subjectClass = new SubjectClass()
            {
                Id = subjectClassCard.Id,
                Code = subjectClassCard.Code,
                StartDate = subjectClassCard.StartDate,
                EndDate = subjectClassCard.EndDate,
                Period = subjectClassCard.Period,
                MaxNumberOfStudents = subjectClassCard.MaxNumberOfStudents,
                IdSubject = subjectClassCard.SelectedSubject?.Id,
                IdTrainingForm = subjectClassCard.SelectedTrainingForm?.Id,
                IdSemester = subjectClassCard.SelectedSemester?.Id,
                WeekDay = DayOfWeeks.IndexOf(subjectClassCard.SelectedDay),
            };

            subjectClass.Teachers?.Clear();
            subjectClass.Teachers?.Add(subjectClassCard.SelectedTeacher);

            return subjectClass;
        }

        public bool SaveSubjectClassCardToDatabase(SubjectClassCard subjectClassCard)
        {
            try
            {
                SubjectClass subjectClass = ConvertSubjectClassCardToSubjectClass(subjectClassCard);

                bool success = SaveSubjectClassToDatabase(subjectClass);

                return success;
            }
            catch (Exception e)
            {
                MyMessageBox.Show(e.Message);
                return false;
            }
        }

        public void UpdateIds(SubjectClass a)
        {
            if (a.Semester != null)
                a.IdSemester = a.Semester.Id;
            if (a.Subject != null)
                a.IdSubject = a.Subject.Id;
            if (a.TrainingForm != null)
                a.IdTrainingForm = a.TrainingForm.Id;
            if (a.DatabaseImageTable != null)
                a.IdThumbnail = a.DatabaseImageTable.Id;
        }

        public List<string> DayOfWeeks = new List<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
    }
}
