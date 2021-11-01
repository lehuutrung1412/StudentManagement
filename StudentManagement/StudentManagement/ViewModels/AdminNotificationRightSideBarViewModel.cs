using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminNotificationViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminNotificationRightSideBarViewModel: BaseViewModel
    {
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


        private object _adminNotificationRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;


        public ICommand ShowCardInfo { get => _showCardInfo; set => _showCardInfo = value; }

        private ICommand _showCardInfo;

        public AdminNotificationRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            ShowCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowCardInfoByCardDataContext(p));
        }


        public void InitRightSideBarItemViewModel()
        {
            this._adminNotificationRightSideBarItemViewModel = new AdminNotificationRightSideBarItemViewModel();
            this._emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            this.RightSideBarItemViewModel = this._emptyStateRightSideBarViewModel;
        }
        public void ShowCardInfoByCardDataContext(UserControl p)
        {
            CardNotification card = p.DataContext as CardNotification;

            this._adminNotificationRightSideBarItemViewModel = new AdminNotificationRightSideBarItemViewModel(card);

            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarItemViewModel;
        }
    }
}
