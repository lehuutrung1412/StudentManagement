using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class StudentCourseRegistryRightSideBarItemViewModel: BaseViewModel
    {
        public SubjectClass CurrentItem { get => _currentItem; set => _currentItem = value; }
        private SubjectClass _currentItem;

        public StudentCourseRegistryRightSideBarItemViewModel()
        {
            this.CurrentItem = null;
        }

        public StudentCourseRegistryRightSideBarItemViewModel(SubjectClass item)
        {
            this.CurrentItem = item;
        }
    }
}
