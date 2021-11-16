using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.CourseRegistry.StudentCourseRegistryRightSideBarViewModel;
using StudentManagement.ViewModels.CourseRegistry;

namespace StudentManagement.ViewModels.ScheduleTable
{
    public class StudentScheduleTableViewModel : BaseViewModel
    {
        public class CourseClass : CourseRegistry.SubjectClass
        {
            private string _day;
            private string _session;
            private string _semester;

            public CourseClass(string idSubjectClass, string subjectName, int credit, string teacherName, DateTime startDate, DateTime endDate, string tKB, string semester) : base(idSubjectClass, subjectName, credit, teacherName, startDate, endDate, tKB)
            {
                string[] temp = TKB.Split(' ');
                Day = temp[3];
                Session = temp[1];
                Semester = semester;
            }
            /*public CourseClass(CourseClass a)
            {
                this.IdSubjectClass = a.IdSubjectClass;
                this.Day = a.Day;
                this.Session = a.Session;
                this.SubjectName = a.SubjectName;
                this.Credit = a.Credit;
                this.TeacherName = a.TeacherName;
                this.StartDate = a.StartDate;
                this.EndDate = a.EndDate;
                this.TKB = a.TKB;
            }*/
            public string Day { get => _day; set => _day = value; }
            public string Session { get => _session; set => _session = value; }
            public string Semester { get => _semester; set => _semester = value; }
        }
        public class ScheduleItem
        {
            private int _start; /*{0, 1, 2, 3, ..., 9}*/
            private int _span;
            private int _day; /*{0, 1, 2, 3, 4, 5, 6}*/
            private string _idSubjectClass;
            private string _SubjectName;
            private int _count;
            // Thiếu startdate, endate, phòng
            public ScheduleItem(CourseClass a)
            {
                string temp = a.Session.Replace("10", "A");
                Span = temp.Length;
                if (temp[0] == 'A')
                    Start = 9;
                else
                    Start = Convert.ToInt32(temp[0] - '0') - 1;
                IdSubjectClass = a.IdSubjectClass;
                SubjectName = a.SubjectName;
                Count = 30;
                Day = Convert.ToInt32(a.Day) - 2; //Sẽ sửa ngay thôi
            }
            public int Start { get => _start; set => _start = value; }
            public int Span { get => _span; set => _span = value; }
            public string IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
            public string SubjectName { get => _SubjectName; set => _SubjectName = value; }
            public int Count { get => _count; set => _count = value; }
            public int Day { get => _day; set => _day = value; }
        }
        private ObservableCollection<CourseClass> _subjectClasses;
        public ObservableCollection<CourseClass> SubjectClasses { get => _subjectClasses; set => _subjectClasses = value; }
        private ObservableCollection<ScheduleItem> _scheduleItems;
        public ObservableCollection<ScheduleItem> ScheduleItems { get => _scheduleItems; set { _scheduleItems = value; OnPropertyChanged(); } }
        public ObservableCollection<string> Semesters { get => _semesters; set => _semesters = value; }

        private ObservableCollection<string> _semesters;
        private string _selectedSemester;
        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                UpdateScheduleItems(value);
            }
        }
        public StudentScheduleTableViewModel()
        {
            SubjectClasses = new ObservableCollection<CourseClass>
            {
                new CourseClass("IT008.L21.KHTN", "Lập trình trực quan", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2", "HK1, 2019-2020"),
                new CourseClass("IT009.L21.KHCL", "Không biết", 2, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 12 Thứ 2", "HK1, 2019-2020"),
                new CourseClass("ENG02.L21", "Anh văn 2", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 3", "HK2, 2019-2020"),
                new CourseClass("ENG03.L21", "Anh văn 3", 4, "Trương Thế Vinh", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 910 Thứ 7", "HK2, 2019-2020")
            };
            Semesters = new ObservableCollection<string>(SubjectClasses.Select(x => x.Semester).Distinct().ToList());
            ScheduleItems = new ObservableCollection<ScheduleItem>();
            UpdateScheduleItems(Semesters.Last());
        }
        public void UpdateScheduleItems(string semester)
        {
            if (semester == null)
                return;
            ScheduleItems = new ObservableCollection<ScheduleItem>();
            foreach (CourseClass item in SubjectClasses)
            {
                if (item.Semester.Contains(semester))
                {
                    ScheduleItem temp = new ScheduleItem(item);
                    ScheduleItems.Add(temp);
                }
            }
        }
    }
}
