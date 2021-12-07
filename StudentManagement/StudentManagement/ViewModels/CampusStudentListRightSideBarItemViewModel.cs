using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Student = StudentManagement.ViewModels.AdminStudentListViewModel.Student;

namespace StudentManagement.ViewModels
{
    public class CampusStudentListRightSideBarItemViewModel : BaseViewModel
    {
        private UserCard _currentStudent;
        public UserCard CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        public CampusStudentListRightSideBarItemViewModel()
        {
            CurrentStudent = null;
        }

        public CampusStudentListRightSideBarItemViewModel(UserCard x)
        {
            CurrentStudent = x;
        }
    }
}
