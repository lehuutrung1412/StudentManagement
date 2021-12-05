using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminStudentListViewModel;
using Student = StudentManagement.ViewModels.AdminStudentListViewModel.Student;

namespace StudentManagement.ViewModels
{
    public class CampusStudentListRightSideBarViewModel : BaseViewModel
    {
        private static CampusStudentListRightSideBarViewModel s_instance;
        public static CampusStudentListRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new CampusStudentListRightSideBarViewModel());

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

        private Student _selectedItem;
        public Student SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private object _campusStudentListRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        

        public ICommand EditStudentInfo { get => _editStudentInfo; set => _editStudentInfo = value; }

        private ICommand _editStudentInfo;

        public void InitRightSideBarItemViewModel()
        {
            _campusStudentListRightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public void InitRightSideBarCommand()
        {
            EditStudentInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditStudentInfoFunction(p));
        }

        void EditStudentInfoFunction(object p)
        {
            Student currentStudent = p as Student;
            _campusStudentListRightSideBarItemViewModel = new CampusStudentListRightSideBarItemEditViewModel(currentStudent);
            RightSideBarItemViewModel = _campusStudentListRightSideBarItemViewModel;
        }

        public CampusStudentListRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitRightSideBarCommand();

            Instance = this;
        }

    }
}
