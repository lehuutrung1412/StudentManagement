using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.ScoreBoardRightSideBar;

namespace StudentManagement.ViewModels
{
    public class ScoreBoardRightSideBarItem : BaseViewModel
    {
        private DetailScore _currentItem;
        public DetailScore CurrentItem { get => _currentItem; set => _currentItem = value; }

        public ScoreBoardRightSideBarItem()
        {
            this.CurrentItem = null;
        }

        public ScoreBoardRightSideBarItem(DetailScore item)
        {
            this.CurrentItem = item;
        }
    }
}



