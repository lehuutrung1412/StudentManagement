using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarViewModel : BaseViewModel
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
        private object _adminCourseRegistryRightSideBarItemEditViewModel;

        private object _emptyStateRightSideBarViewModel;

        public AdminCourseRegistryRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            SubjectClasses = new ObservableCollection<TempSubjectClass>
            {
                new TempSubjectClass("IT008.L21.KHTN 1", "Lập trình trực quan", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("IT009.L21.KHCL 1", "Không biết", 2, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("ENG02.L21 1", "Anh văn 2", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("IT008.L21.KHTN 2", "Lập trình trực quan", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("IT009.L21.KHCL 2", "Không biết", 2, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new TempSubjectClass("ENG02.L21 2", "Anh văn 2", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
            };
        }


        public void InitRightSideBarItemViewModel()
        {
            _studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

    }
    
}
