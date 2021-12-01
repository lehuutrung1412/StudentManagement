using StudentManagement.Models;
using System;
using System.Collections.Generic;
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
            set { _isSelected = value; OnPropertyChanged(); }
        }
        public CourseItem(Models.SubjectClass a, bool isSelected)
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
        }
        public SubjectClass ConvertToSubjectClass()
        {
            SubjectClass temp = new SubjectClass()
            {
                Id = this.Id,
                Teachers = this.Teachers,
                Semester = this.Semester,
                IdSemester = this.IdSemester,
                Subject = this.Subject,
                IdSubject = this.IdSubject,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Period = this.Period,
                WeekDay = this.WeekDay,
                Code = this.Code,
                TrainingForm = this.TrainingForm,
                IdTrainingForm = this.IdTrainingForm,
                NumberOfStudents = this.NumberOfStudents,
                MaxNumberOfStudents = this.MaxNumberOfStudents,
                IdThumbnail = this.IdThumbnail,
                DatabaseImageTable = this.DatabaseImageTable,
        };
            return temp;
        }
    }
}
