using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarItemViewModel : BaseViewModel
    {
        public SubjectClass CurrentItem { get => _currentItem; set => _currentItem = value; }
        private SubjectClass _currentItem;
        
        public AdminCourseRegistryRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public AdminCourseRegistryRightSideBarItemViewModel(SubjectClass item)
        {
            CurrentItem = item;
        }
    }
}
