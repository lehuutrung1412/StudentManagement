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
                SelectedClass = SubjectClasses.Where(x => x.IdSubjectClass == SelectedItem.IdSubjectClass) as SubjectClass;
                this._studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel(SelectedClass);
                this.RightSideBarItemViewModel = this._studentCourseRegistryRightSideBarItemViewModel;
            }
        }
        private SubjectClass _selectedClass;
        public SubjectClass SelectedClass
        {
            get => _selectedClass; set
            {
                _selectedClass = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<SubjectClass> _subjectClasses;
        public ObservableCollection<SubjectClass> SubjectClasses { get => _subjectClasses; set => _subjectClasses = value; }


        private object _studentCourseRegistryRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        public StudentCourseRegistryRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            SubjectClasses = new ObservableCollection<SubjectClass>
            {
                new SubjectClass("IT008.L21.KHTN", "Lập trình trực quan", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new SubjectClass("IT009.L21.KHCL", "Không biết", 2, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
                new SubjectClass("ENG02.L21", "Anh văn 2", 4, "Trương Tấn Toàn", new DateTime(2021, 1, 1), new DateTime(2021, 1, 30), "Tiết 678 Thứ 2"),
            };
        }


        public void InitRightSideBarItemViewModel()
        {
            this._studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel();
            this._emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            this.RightSideBarItemViewModel = this._emptyStateRightSideBarViewModel;
        }
    }
}
