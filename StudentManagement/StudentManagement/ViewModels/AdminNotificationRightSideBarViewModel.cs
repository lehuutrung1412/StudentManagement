using StudentManagement.Commands;
using StudentManagement.Views;
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

        private CardNotification _card;

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

        public ICommand UpdateNotificationCommand { get => _updateNotification; set => _updateNotification = value; }

        private ICommand _updateNotification;

        public ICommand DeleteNotificationCommand { get => _deleteNotification; set => _deleteNotification = value; }

        private ICommand _deleteNotification;
        public AdminNotificationRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            _card = null;


            ShowCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowCardInfoByCardDataContext(p));
            Editnotification = new RelayCommand<object>((p) => { return true; }, (p) => EditnotificationByCardDataContext());
            CancelNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelNotification());
            UpdateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateNotification());
            DeleteNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteNotification());
        }


        public void InitRightSideBarItemViewModel()
        {
            this._adminNotificationRightSideBarItemViewModel = new AdminNotificationRightSideBarItemViewModel();
            this._emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            this.RightSideBarItemViewModel = this._emptyStateRightSideBarViewModel;
        }
        public void ShowCardInfoByCardDataContext(UserControl p)
        {
            _card = p.DataContext as CardNotification;

            this._adminNotificationRightSideBarItemViewModel = new AdminNotificationRightSideBarItemViewModel(_card);
            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarItemViewModel;
        }
        public void EditnotificationByCardDataContext()
        {
            this._adminNotificationRightSideBarEditViewModel = new AdminNotificationRightSideBarEdit();
            (this._adminNotificationRightSideBarEditViewModel as AdminNotificationRightSideBarEdit).CurrentCard = new CardNotification((this._adminNotificationRightSideBarItemViewModel as AdminNotificationRightSideBarItemViewModel).CurrentCard);
            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarEditViewModel;
        }
        public void CancelNotification()
        {
            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarItemViewModel;
        }
        public void UpdateNotification()
        {
            CardNotification card = new CardNotification((this._adminNotificationRightSideBarEditViewModel as AdminNotificationRightSideBarEdit).CurrentCard);
            (this._adminNotificationRightSideBarItemViewModel as AdminNotificationRightSideBarItemViewModel).CurrentCard = card;
            this.RightSideBarItemViewModel = this._adminNotificationRightSideBarItemViewModel;

            AdminNotification NotificationUserControl = new AdminNotification();
            if (NotificationUserControl.DataContext == null)
                return;
            var NotificationVM = NotificationUserControl.DataContext as AdminNotificationViewModel;
            for (int i = 0; i < NotificationVM.Cards.Count; i++)
                if (NotificationVM.Cards[i].Id == card.Id)
                {
                    NotificationVM.Cards[i] = card;
                    break;
                }
            for (int i = 0; i < NotificationVM.RealCards.Count; i++)
                if (NotificationVM.RealCards[i].Id == card.Id)
                {
                    NotificationVM.RealCards[i] = card;
                    break;
                }
        }
        public void DeleteNotification()
        {
            if (MyMessageBox.Show("Bạn có chắc muốn xoá thông báo này", "Thông báo", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.OK)
                return;
            AdminNotification NotificationUserControl = new AdminNotification();
            if (NotificationUserControl.DataContext == null)
                return;
            var NotificationVM = NotificationUserControl.DataContext as AdminNotificationViewModel;
            var tmp = NotificationVM.Cards.Where(x => x.Id == _card.Id).FirstOrDefault();
            NotificationVM.Cards.Remove(tmp);
            NotificationVM.RealCards.Remove(tmp);
        }
    }
}
