using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.StudentListRightSideBarViewModel;

namespace StudentManagement.ViewModels
{
    public class StudentListRightSideBarItemViewModel : BaseViewModel
    {
        private DetailScore _currentScore;
        public DetailScore CurrentScore { get => _currentScore; set => _currentScore = value; }

        public StudentListRightSideBarItemViewModel()
        {
            CurrentScore = null;
        }

        public StudentListRightSideBarItemViewModel(DetailScore x)
        {
            CurrentScore = x;
        }
    }
}
