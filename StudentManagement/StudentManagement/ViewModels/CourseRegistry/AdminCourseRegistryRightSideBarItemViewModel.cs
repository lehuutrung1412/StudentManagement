using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarItemViewModel : BaseViewModel
    {
        public CourseItem CurrentItem { get => _currentItem; set => _currentItem = value; }
        private CourseItem _currentItem;
        
        public AdminCourseRegistryRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public AdminCourseRegistryRightSideBarItemViewModel(CourseItem item)
        {
            CurrentItem = item;
        }
    }
}
