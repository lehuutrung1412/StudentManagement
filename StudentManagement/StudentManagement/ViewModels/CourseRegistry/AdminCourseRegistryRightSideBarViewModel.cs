using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarViewModel : BaseViewModel
    {
        private static AdminCourseRegistryRightSideBarViewModel s_instance;
        public static AdminCourseRegistryRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminCourseRegistryRightSideBarViewModel());

            private set => s_instance = value;
        }

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
                    
                    _adminCourseRegistryRightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemViewModel(_selectedItem);
                    RightSideBarItemViewModel = _adminCourseRegistryRightSideBarItemViewModel;
                }
            }
        }
        
        

        private object _adminCourseRegistryRightSideBarItemEditViewModel;
        private object _adminCourseRegistryRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        private bool _canEdit;
        public bool CanEdit { get => _canEdit; set { 
                _canEdit = value; OnPropertyChanged(); } }

        #region Commands
        public ICommand EditCourseInfoCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        #endregion
        public AdminCourseRegistryRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitCommand();
            Instance = this;
        }

        public void InitCommand()
        {
            EditCourseInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => EditCourseInfo(p));
            DeleteCourseCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteCourse(p));
        }
        public void InitRightSideBarItemViewModel()
        {
            _adminCourseRegistryRightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemViewModel();
            _adminCourseRegistryRightSideBarItemEditViewModel = new AdminCourseRegistryRightSideBarItemEditViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public void EditCourseInfo(object p)
        {
            /*SubjectCard card = p as SubjectCard;

            _adminSubjectClassRightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemEditViewModel(card);

            RightSideBarItemViewModel = _adminSubjectClassRightSideBarItemViewModel;*/
            CourseItem item = p as CourseItem;
            _adminCourseRegistryRightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemEditViewModel(item);
            RightSideBarItemViewModel = _adminCourseRegistryRightSideBarItemViewModel;
            
        }

        public void DeleteCourse(object p)
        {
            /*SubjectClass card = p as SubjectCard;

            SubjectCards.Remove(card);
            StoredSubjectCards.Remove(card);

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;*/
        }
    }
    
}
