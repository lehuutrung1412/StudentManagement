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

        private object _adminNotificationRightSideBarEditViewModel;

        private object _emptyStateRightSideBarViewModel;


        public ICommand ShowCardInfo { get => _showCardInfo; set => _showCardInfo = value; }

        private ICommand _showCardInfo;

        public ICommand Editnotification { get => _editNotification; set => _editNotification = value; }

        private ICommand _editNotification;

        public ICommand CancelNotificationCommand { get => _cancelNotification; set => _cancelNotification = value; }

        private ICommand _cancelNotification;
        public AdminNotificationRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            ShowCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowCardInfoByCardDataContext(p));
            Editnotification = new RelayCommand<object>((p) => { return true; }, (p) => EditnotificationByCardDataContext());
            CancelNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelNotification());
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

            this._adminNotificationRightSideBarEditViewModel = new AdminNotificationRightSideBarEdit(card);

            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarItemViewModel;
        }
        public void EditnotificationByCardDataContext()
        { 
            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarEditViewModel;
        }
        public void CancelNotification()
        {
            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarItemViewModel;
        }

    }
}
