using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class CampusStudentListRightSideBarItemViewModel : BaseViewModel
    {
        private ObservableCollection<InfoItem> _currentStudent;
        public ObservableCollection<InfoItem> CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        public CampusStudentListRightSideBarItemViewModel()
        {
            CurrentStudent = null;
        }
        
        public CampusStudentListRightSideBarItemViewModel(ObservableCollection<InfoItem> x)
        {
            CurrentStudent = x;
        }
    }
}
