using StudentManagement.Commands;
using StudentManagement.Objects;
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
        private CourseItem _selectedItem;
        public CourseItem SelectedItem
        {
            get => _selectedItem; set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (_selectedItem != null)
                {
                    _studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel(_selectedItem);
                    RightSideBarItemViewModel = _studentCourseRegistryRightSideBarItemViewModel;
                    try
                    {
                        ScheduleItem wilRemoveSchedule = StudentCourseRegistryViewModel.Instance.SelectedScheduleItem2;
                        StudentCourseRegistryViewModel.Instance.ScheduleItemsRegistered.Remove(wilRemoveSchedule);
                    }
                    catch { }
                    int tempType = SelectedItem.IsConflict ? 1 : 0;
                    StudentCourseRegistryViewModel.Instance.SelectedScheduleItem2 = new ScheduleItem(SelectedItem.ConvertToSubjectClass(), true, SelectedItem.IsConflict, 1+tempType);
                }

            }
        }
        private object _studentCourseRegistryRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        public StudentCourseRegistryRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
        }


        public void InitRightSideBarItemViewModel()
        {
            _studentCourseRegistryRightSideBarItemViewModel = new StudentCourseRegistryRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

    }
}
