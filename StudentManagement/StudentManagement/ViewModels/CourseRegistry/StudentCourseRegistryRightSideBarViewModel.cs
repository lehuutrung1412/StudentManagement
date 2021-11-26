using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class StudentCourseRegistryRightSideBarViewModel : BaseViewModel
    {
        private object _rightSideBarItemViewModel;

        public object RightSideBarItemViewModel
        {
            get { return _rightSideBarItemViewModel; }
            set
            {
                _rightSideBarItemViewModel = value;
                OnPropertyChanged();
            }
        }
        private CourseRegistryItem _selectedItem;
        public CourseRegistryItem SelectedItem
        {
            get => _selectedItem; set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (_selectedItem != null)
                {
                    int count = SubjectClasses.Where(x => x.IdSubjectClass == SelectedItem.IdSubjectClass).Count();
                    SelectedClass = SubjectClasses.Where(x => x.IdSubjectClass == SelectedItem.IdSubjectClass).ToList()[0];
                    _studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel(SelectedClass);
                    RightSideBarItemViewModel = _studentCourseRegistryRightSideBarItemViewModel;
                }

            }
        }
        private TempSubjectClass _selectedClass;
        public TempSubjectClass SelectedClass
        {
            get => _selectedClass; set
            {
                _selectedClass = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TempSubjectClass> _subjectClasses;
        public ObservableCollection<TempSubjectClass> SubjectClasses { get => _subjectClasses; set => _subjectClasses = value; }


        private object _studentCourseRegistryRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        public StudentCourseRegistryRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            SubjectClasses = new ObservableCollection<TempSubjectClass>
            {
                new TempSubjectClass("IT008.L21.KHTN", "Lập trình trực quan", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("IT009.L21.KHCL", "Không biết", 2, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("ENG02.L21", "Anh văn 2", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
            };
        }


        public void InitRightSideBarItemViewModel()
        {
            _studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

    }

    public class TempSubjectClass
    {
        private string _idSubjectClass;
        private string _subjectName;
        private int _credit;
        private string _teacherName;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _tKB;
        public TempSubjectClass(string idSubjectClass, string subjectName, int credit, string teacherName, DateTime startDate, DateTime endDate, string tKB)
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
}
