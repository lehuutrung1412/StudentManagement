using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    class AdminHomeRightSideBarViewModel : BaseViewModel
    {
        #region properties
        private object _rightSideBarItemViewModel;

        public object RightSideBarItemViewModel
        {
            get { return _rightSideBarItemViewModel; }
            set
            {
                _rightSideBarItemViewModel = value;
                OnPropertyChanged();
            }
        }

        private object _emptyStateRightSideBarViewModel;
        #endregion

        public AdminHomeRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();

        }

        #region methods
        public void InitRightSideBarItemViewModel()
        {
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
        #endregion
    }
}
