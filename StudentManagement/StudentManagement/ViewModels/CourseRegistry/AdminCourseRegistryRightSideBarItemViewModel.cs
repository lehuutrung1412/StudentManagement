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
        public CourseItems CurrentItem { get => _currentItem; set => _currentItem = value; }
        private CourseItems _currentItem;
        
        public AdminCourseRegistryRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public AdminCourseRegistryRightSideBarItemViewModel(CourseItems item)
        {
            CurrentItem = item;
        }
    }
}
