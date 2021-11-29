using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class NewsfeedRightSideBarViewModel : BaseViewModel
    {
        public ObservableCollection<DateTime> ScheduleTimes { get; set; }
        public ObservableCollection<DateTime> AbsentTimes { get; set; }
        public ObservableCollection<DateTime> MakeUpTimes { get; set; }

        public NewsfeedRightSideBarViewModel()
        {
            ScheduleTimes = new ObservableCollection<DateTime> { new DateTime(2021, 11, 28), new DateTime(2021, 11, 21), new DateTime(2021, 12, 21) };
        }
    }
}
