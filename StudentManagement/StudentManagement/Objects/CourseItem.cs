using StudentManagement.Models;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class CourseItem : Models.SubjectClass
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set 
            { 
                _isSelected = value; 
                OnPropertyChanged(); 
                if (value)
                {
                    _isSelected = !(IsConflict || IsValidSubject) ;
                }
            }
        }
        private bool _isConflict;
        public bool IsConflict
        {
            get => _isConflict;
            set { _isConflict = value; OnPropertyChanged(); }
        }

        private bool _isValidSubject;
        public bool IsValidSubject
        {
            get => _isValidSubject;
            set { _isValidSubject = value; OnPropertyChanged(); }
        }

        private Teacher _mainTeacher;
        public Teacher MainTeacher { get => _mainTeacher; set { _mainTeacher = value; OnPropertyChanged(); } }

        public CourseItem(Models.SubjectClass a, bool isSelected, bool isConflict = false, bool isValidSubject = false)
        {
            this.Id = a.Id;
            this.Teachers = a.Teachers;
            this.Semester = a.Semester;
            this.IdSemester = a.IdSemester;
            this.Subject = a.Subject;
            this.IdSubject = a.IdSubject;
            this.StartDate = a.StartDate;
            this.EndDate = a.EndDate;
            this.Period = a.Period;
            this.WeekDay = a.WeekDay;
            this.Code = a.Code;
            this.TrainingForm = a.TrainingForm;
            this.IdTrainingForm = a.IdTrainingForm;
            this.NumberOfStudents = a.NumberOfStudents;
            this.MaxNumberOfStudents = a.MaxNumberOfStudents;
            this.IdThumbnail = a.IdThumbnail;
            this.DatabaseImageTable = a.DatabaseImageTable;
            this.IsSelected = false;
            this.IsConflict = isConflict;
            this.IsValidSubject = isValidSubject;
            this.MainTeacher = this.Teachers.FirstOrDefault();
        }
        public SubjectClass ConvertToSubjectClass()
        {
            SubjectClass temp = new SubjectClass()
            {
                Id = this.Id,
                Teachers = this.Teachers,
                Semester = this.Semester,
                Subject = this.Subject,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Period = this.Period,
                WeekDay = this.WeekDay,
                Code = this.Code,
                TrainingForm = this.TrainingForm,
                NumberOfStudents = this.NumberOfStudents,
                MaxNumberOfStudents = this.MaxNumberOfStudents,
                DatabaseImageTable = this.DatabaseImageTable,
            };
            SubjectClassServices.Instance.UpdateIds(temp);
            return temp;
        }

        public static ObservableCollection<CourseItem> ConvertToListCourseItem(ObservableCollection<SubjectClass> listSubjectClass)
        {
            ObservableCollection<CourseItem> result = new ObservableCollection<CourseItem>();
            foreach(SubjectClass subjectClass in listSubjectClass)
            {
                result.Add(new CourseItem(subjectClass, false));
            }
            return result;
        }
        public static ObservableCollection<SubjectClass> ConvertToListSubjectClass(ObservableCollection<CourseItem> listCourseItem)
        {
            ObservableCollection<SubjectClass> result = new ObservableCollection<SubjectClass>();
            foreach (CourseItem course in listCourseItem)
            {
                result.Add(course.ConvertToSubjectClass());
            }
            return result;
        }
        public static bool IsConflictCourseRegistry(ObservableCollection<CourseItem> listCourse, CourseItem course)
        {
            foreach (CourseItem listElement in listCourse)
            {
                if (course.WeekDay == listElement.WeekDay)
                {
                    foreach (char period in listElement.Period)
                    {
                        if (course.Period.Contains(period))
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool IsSameSubjectCourseRegistry(ObservableCollection<CourseItem> listCourse, CourseItem course)
        {
            foreach (CourseItem listElement in listCourse)
            {
                if (course.Subject.Id == listElement.Subject.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEqualProperty(SubjectClass a)
        {
            return
            this.Teachers.FirstOrDefault() == a.Teachers.FirstOrDefault() &&
            this.Semester == a.Semester &&
            this.Subject == a.Subject &&
            this.StartDate.Value.Date == a.StartDate.Value.Date &&
            this.EndDate.Value.Date == a.EndDate.Value.Date &&
            this.Period == a.Period &&
            this.WeekDay == a.WeekDay &&
            this.Code == a.Code &&
            this.TrainingForm == a.TrainingForm &&
            this.MaxNumberOfStudents == a.MaxNumberOfStudents &&
            this.IdThumbnail == a.IdThumbnail &&
            this.DatabaseImageTable == a.DatabaseImageTable;
        }
    }
}
