using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.StudentListRightSideBarViewModel;
using Student = StudentManagement.ViewModels.AdminStudentListViewModel.Student;

namespace StudentManagement.ViewModels
{
    public class StudentListRightSideBarItemViewModel : BaseViewModel
    {
        private DetailScore _currentScore;
        public DetailScore CurrentScore { get => _currentScore; set => _currentScore = value; }

        private Student _currentStudent;
        public Student CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        public StudentListRightSideBarItemViewModel()
        {
            CurrentScore = null;
            CurrentStudent = null;
        }

        public StudentListRightSideBarItemViewModel(DetailScore x, Student y)
        {
            CurrentScore = x;
            CurrentStudent = y;
        }
    }
}
