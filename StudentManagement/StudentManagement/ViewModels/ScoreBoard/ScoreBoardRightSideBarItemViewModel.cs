using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.ScoreBoardRightSideBarViewModel;

namespace StudentManagement.ViewModels
{
    public class ScoreBoardRightSideBarItemViewModel : BaseViewModel
    {
        private DetailScore _currentItem;
        public DetailScore CurrentItem { get => _currentItem; set => _currentItem = value; }

        public ScoreBoardRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public ScoreBoardRightSideBarItemViewModel(DetailScore item)
        {
            CurrentItem = item;
        }
    }
}



