﻿using StudentManagement.Commands;
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
    public class AdminNotificationRightSideBarViewModel : BaseViewModel
    {
        private static AdminNotificationRightSideBarViewModel s_instance;
        public static AdminNotificationRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminNotificationRightSideBarViewModel());

            private set => s_instance = value;
        }
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


        public object _adminNotificationRightSideBarItemViewModel;

        public object _adminNotificationRightSideBarEditViewModel;

        public object _emptyStateRightSideBarViewModel;


        public ICommand ShowCardInfo { get => _showCardInfo; set => _showCardInfo = value; }

        private ICommand _showCardInfo;

        public ICommand Editnotification { get => _editNotification; set => _editNotification = value; }

        private ICommand _editNotification;

        public ICommand CancelNotificationCommand { get => _cancelNotification; set => _cancelNotification = value; }

        private ICommand _cancelNotification;

        private CardNotification _currentCard;
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        public AdminNotificationRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            CurrentCard = null;
            ShowCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowCardInfoByCardDataContext(p));
            Editnotification = new RelayCommand<object>((p) => { return true; }, (p) => EditnotificationByCardDataContext());
            CancelNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelNotification());
            Instance = this;
        }


        public void InitRightSideBarItemViewModel()
        {
            _adminNotificationRightSideBarItemViewModel = new AdminNotificationRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
        public void ShowCardInfoByCardDataContext(UserControl p)
        {
            CurrentCard = p.DataContext as CardNotification;

            _adminNotificationRightSideBarItemViewModel = new AdminNotificationRightSideBarItemViewModel(CurrentCard);
            RightSideBarItemViewModel = _adminNotificationRightSideBarItemViewModel;
        }
        public void EditnotificationByCardDataContext()
        {
            var tmp = new CardNotification((_adminNotificationRightSideBarItemViewModel as AdminNotificationRightSideBarItemViewModel).CurrentCard);
            _adminNotificationRightSideBarEditViewModel = new AdminNotificationRightSideBarEditViewModel(tmp);
            CurrentCard = tmp;
            RightSideBarItemViewModel = _adminNotificationRightSideBarEditViewModel;
        }
        public void CancelNotification()
        {
            RightSideBarItemViewModel = _adminNotificationRightSideBarItemViewModel;
        }
    }
}
