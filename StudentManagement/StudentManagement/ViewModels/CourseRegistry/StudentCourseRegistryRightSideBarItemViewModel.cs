using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.StudentCourseRegistryRightSideBarViewModel;

namespace StudentManagement.ViewModels
{
    public class StudentCourseRegistryRightSideBarItemViewModel : BaseViewModel
    {
        public SubjectClass CurrentItem { get => _currentItem; set => _currentItem = value; }
        private SubjectClass _currentItem;

        public StudentCourseRegistryRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public StudentCourseRegistryRightSideBarItemViewModel(SubjectClass item)
        {
            CurrentItem = item;
        }
    }
}
