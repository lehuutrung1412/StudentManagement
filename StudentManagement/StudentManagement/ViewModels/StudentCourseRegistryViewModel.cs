using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class StudentCourseRegistryViewModel: BaseViewModel
    {
       
        private int _totalCredit;
        public int TotalCredit
        {
            get => _totalCredit; set
            {
                _totalCredit = value;
                OnPropertyChanged();
            }
        }
        private bool _isAllItemsSelected2;
        public bool IsAllItemsSelected2
        {
            get => _isAllItemsSelected2; set
            {
                _isAllItemsSelected2 = value;
                OnPropertyChanged();
                if (value == true)
                    CourseRegistryItems2.Select(c => { c.IsSelected = true; return c; }).ToList();
                else
                    CourseRegistryItems2.Select(c => { c.IsSelected = false; return c; }).ToList();
            }
        }
        public class CourseRegistryItem
        {
            private bool _isSelected;
            private string _idSubjectClass;
            private string _subjectName;
            private int _credit;
            private int _limitStudentCount;
            private int _registeredCount;

            public CourseRegistryItem(bool isSelected, string idSubjectClass, string subjectName, int credit, int limitStudentCount, int registeredCount)
            {
                IsSelected = isSelected;
                IdSubjectClass = idSubjectClass;
                SubjectName = subjectName;
                Credit = credit;
                LimitStudentCount = limitStudentCount;
                RegisteredCount = registeredCount;
            }

            public bool IsSelected { get => _isSelected; set => _isSelected = value; }
            public string IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
            public string SubjectName { get => _subjectName; set => _subjectName = value; }
            public int Credit { get => _credit; set => _credit = value; }
            public int LimitStudentCount { get => _limitStudentCount; set => _limitStudentCount = value; }
            public int RegisteredCount { get => _registeredCount; set => _registeredCount = value; }
        }
        public class SubjectClass
        {
            private string _idSubjectClass;
            private string _subjectName;
            private int _credit;
            private string _teacherName;
            private DateTime _startDate;
            private DateTime _endDate;
            private string _tKB;
            public SubjectClass(string idSubjectClass, string subjectName, int credit, string teacherName, DateTime startDate, DateTime endDate, string tKB)
            {
                IdSubjectClass = idSubjectClass;
                SubjectName = subjectName;
                Credit = credit;
                TeacherName = teacherName;
                StartDate = startDate;
                EndDate = endDate;
                TKB = tKB;
            }
            public string IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
            public string SubjectName { get => _subjectName; set => _subjectName = value; }
            public int Credit { get => _credit; set => _credit = value; }
            public string TeacherName { get => _teacherName; set => _teacherName = value; }
            public DateTime StartDate { get => _startDate; set => _startDate = value; }
            public DateTime EndDate { get => _endDate; set => _endDate = value; }
            public string TKB { get => _tKB; set => _tKB = value; }
        }
        private ObservableCollection<CourseRegistryItem> courseRegistryItems;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItems { get => courseRegistryItems; set => courseRegistryItems = value; }
        private ObservableCollection<CourseRegistryItem> courseRegistryItems2;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItems2 { get => courseRegistryItems2; set => courseRegistryItems2 = value; }

        public StudentCourseRegistryViewModel()
        {
            CourseRegistryItems = new ObservableCollection<CourseRegistryItem>
            {
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(true, "ENG02.L21", "Anh văn 2", 4, 30, 28)
            };
            CourseRegistryItems2 = new ObservableCollection<CourseRegistryItem>
            {
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(true, "ENG02.L21", "Anh văn 2", 4, 30, 28),
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(true, "ENG02.L21", "Anh văn 2", 4, 30, 28),
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(true, "ENG02.L21", "Anh văn 2", 4, 30, 28)
            };
            
            TotalCredit = CourseRegistryItems.Sum(x => x.Credit);

        }
    }
}
