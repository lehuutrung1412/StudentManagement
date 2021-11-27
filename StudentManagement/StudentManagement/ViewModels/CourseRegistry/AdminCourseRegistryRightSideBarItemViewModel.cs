using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarItemViewModel : BaseViewModel
    {
        public TempSubjectClass CurrentItem { get => _currentItem; set => _currentItem = value; }
        private TempSubjectClass _currentItem;
        
        public AdminCourseRegistryRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public AdminCourseRegistryRightSideBarItemViewModel(TempSubjectClass item)
        {
            CurrentItem = item;
        }
    }
}
