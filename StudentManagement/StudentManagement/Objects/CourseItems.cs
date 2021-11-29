using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class CourseItems : Models.SubjectClass
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }
        public CourseItems(Models.SubjectClass a, bool isSelected)
        {
            this.Id = a.Id;
            this.Teachers = a.Teachers;
            this.Semester = a.Semester;
            this.Subject = a.Subject;
            this.StartDate = a.StartDate;
            this.EndDate = a.EndDate;
            this.Period = a.Period;
            this.WeekDay = a.WeekDay;
            /*thiếu
            this.Code = a.Code;
            this.TrainingForm = a.TrainingForm;
            this.NumberStudent = a.NumberStudent;
            this.MaxOfRegistry = a.MaxOfRegistry;*/
            this.IsSelected = false;
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
                /*thiếu
                Code = this.Code,
                TrainingForm = this.TrainingForm;
                NumberStudent = this.NumberStudent;
                MaxOfRegistry = this.MaxOfRegistry;*/

            };
            return temp;
        }
    }
}
