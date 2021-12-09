using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class CampusStudentListRightSideBarItemViewModel : BaseViewModel
    {
        private StudentGrid _currentStudent;
        public StudentGrid CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        public CampusStudentListRightSideBarItemViewModel()
        {
            CurrentStudent = null;
        }

        public CampusStudentListRightSideBarItemViewModel(StudentGrid x)
        {
            CurrentStudent = x;
        }
    }
}
