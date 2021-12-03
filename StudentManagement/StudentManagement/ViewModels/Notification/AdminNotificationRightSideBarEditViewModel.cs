using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminNotificationViewModel;

namespace StudentManagement.ViewModels
{
    class AdminNotificationRightSideBarEditViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Properties
        public NotificationCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private NotificationCard _currentCard;

        private readonly ErrorBaseViewModel _errorBaseViewModel;
        private string _Topic;
        private string _Type;

        public string Topic
        {
            get => _Topic;
            set
            {
                _Topic = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Topic))
                {
                    _errorBaseViewModel.AddError(nameof(Topic), "Vui lòng nhập chủ đề!");
                }

                OnPropertyChanged();
            }

        }
        public string Type
        {
            get => _Type;
            set
            {
                _Type = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Type))
                {
                    _errorBaseViewModel.AddError(nameof(Type), "Vui lòng nhập loại bài đăng!");
                }

                OnPropertyChanged();
            }

        }

        public bool CanUpdate => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;
        #endregion
        #region Icommnad
        public ICommand UpdateNotificationCommand { get => _updateNotification; set => _updateNotification = value; }
        private ICommand _updateNotification;
        #endregion
        public AdminNotificationRightSideBarEditViewModel()
        {
            CurrentCard = null;
        }
        public AdminNotificationRightSideBarEditViewModel(NotificationCard card)
        {
            CurrentCard = card;
            UpdateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateNotification());
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
            Topic = CurrentCard.Topic;
            Type = CurrentCard.Type;
        }
        #region Method
        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanUpdate));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }
        public void UpdateNotification()
        {
            var AdminNotificationRightSideBarVM = AdminNotificationRightSideBarViewModel.Instance;
            NotificationCard card = CurrentCard;
            (AdminNotificationRightSideBarVM._adminNotificationRightSideBarItemViewModel as AdminNotificationRightSideBarItemViewModel).CurrentCard = card;
            AdminNotificationRightSideBarVM.RightSideBarItemViewModel = AdminNotificationRightSideBarVM._adminNotificationRightSideBarItemViewModel;
            CurrentCard.Topic = Topic;
            CurrentCard.Type = Type;
            var AdminNotificationVM = Instance;
            for (int i = 0; i < AdminNotificationVM.Cards.Count; i++)
                if (AdminNotificationVM.Cards[i].Id == card.Id)
                {
                    AdminNotificationVM.Cards[i] = card;
                    break;
                }
            for (int i = 0; i < AdminNotificationVM.RealCards.Count; i++)
                if (AdminNotificationVM.RealCards[i].Id == card.Id)
                {
                    AdminNotificationVM.RealCards[i] = card;
                    break;
                }
            NotificationServices.Instance.UpdateNotificationByNotificationCard(CurrentCard);
        }
        #endregion
    }
}