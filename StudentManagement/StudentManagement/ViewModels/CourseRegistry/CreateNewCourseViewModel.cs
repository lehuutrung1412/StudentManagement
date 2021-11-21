using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.AdminCourseRegistryViewModel;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class CreateNewCourseViewModel
    {
        private CourseRegistryItem _currentCard;
        public CourseRegistryItem CurrentCard { get => _currentCard; set => _currentCard = value; }
        public CreateNewCourseViewModel(CourseRegistryItem card, TempSemester temp, ObservableCollection<CourseRegistryItem> list)
        {
            
        }

        
    }
}
