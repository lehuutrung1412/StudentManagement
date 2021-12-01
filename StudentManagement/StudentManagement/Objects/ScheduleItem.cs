using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class ScheduleItem : BaseViewModel
    {
        private int _start; /*{0, 1, 2, 3, ..., 9}*/
        private int _span;
        private int _day; /*{0, 1, 2, 3, 4, 5, 6}*/
        private string _subjectClassCode;
        private string _SubjectName;
        private string _count;
        private string _startDate;
        private string _endDate;
        private string _teacherName;
        public ScheduleItem(SubjectClass a)
        {
            string temp = a.Period.Replace("10", "A");
            this.Span = temp.Length;
            if (temp[0] == 'A')
                this.Start = 9;
            else
                this.Start = Convert.ToInt32(temp[0] - '0') - 1;
            List<string> DaysofWeek = new List<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
            Day = DaysofWeek.IndexOf(a.WeekDay);
            this.SubjectClassCode = a.Code;
            this.SubjectName = a.Subject.DisplayName;
            this.Count = a.NumberOfStudents.ToString();
            this.StartDate = String.Format("{0:dd/MM/yyyy}", a.StartDate);
            this.EndDate = String.Format("{0:dd/MM/yyyy}", a.EndDate);
            if (a.Teachers.Count == 0)
                TeacherName = "Chưa có";
            else
                this.TeacherName = a.Teachers.FirstOrDefault().User.DisplayName;
        }

        public int Start { get => _start; set { _start = value; OnPropertyChanged(); } }
        public int Span { get => _span; set { _span = value; OnPropertyChanged(); } }
        public int Day { get => _day; set { _day = value; OnPropertyChanged(); } }
        public string SubjectClassCode { get => _subjectClassCode; set { _subjectClassCode = value; OnPropertyChanged(); } }
        public string SubjectName { get => _SubjectName; set { _SubjectName = value; OnPropertyChanged(); } }
        public string Count { get => _count; set { _count = value; OnPropertyChanged(); } }
        public string StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public string EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }
        public string TeacherName { get => _teacherName; set { _teacherName = value; OnPropertyChanged(); } }
    }
}
