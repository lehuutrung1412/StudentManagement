using StudentManagement.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.Notification.AdminNotificationViewModel;

namespace StudentManagement.ViewModels.Notification
{
    class AdminNotificationRightSideBarEditViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        private readonly ErrorBaseViewModel _errorBaseViewModel;
        private string _chuDe;
        private string _loaiBaiDang;

        public string ChuDe
        {
            get => _chuDe;
            set
            {
                _chuDe = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(ChuDe))
                {
                    _errorBaseViewModel.AddError(nameof(ChuDe), "Vui lòng nhập chủ đề!");
                }

                OnPropertyChanged();
            }

        }
        public string LoaiBaiDang
        {
            get => _loaiBaiDang;
            set
            {
                _loaiBaiDang = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(LoaiBaiDang))
                {
                    _errorBaseViewModel.AddError(nameof(LoaiBaiDang), "Vui lòng nhập loại bài đăng!");
                }

                OnPropertyChanged();
            }

        }

        public bool CanUpdate => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public ICommand UpdateNotificationCommand { get => _updateNotification; set => _updateNotification = value; }
        private ICommand _updateNotification;

        public AdminNotificationRightSideBarEditViewModel()
        {
            CurrentCard = null;

        }
        public AdminNotificationRightSideBarEditViewModel(CardNotification card)
        {
            CurrentCard = card;
            UpdateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateNotification());
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
            ChuDe = CurrentCard.ChuDe;
            LoaiBaiDang = CurrentCard.LoaiBaiDang;
        }
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
            CardNotification card = (AdminNotificationRightSideBarVM._adminNotificationRightSideBarEditViewModel as AdminNotificationRightSideBarEditViewModel).CurrentCard;
            (AdminNotificationRightSideBarVM._adminNotificationRightSideBarItemViewModel as AdminNotificationRightSideBarItemViewModel).CurrentCard = card;
            AdminNotificationRightSideBarVM.RightSideBarItemViewModel = AdminNotificationRightSideBarVM._adminNotificationRightSideBarItemViewModel;
            CurrentCard.ChuDe = ChuDe;
            CurrentCard.LoaiBaiDang = LoaiBaiDang;
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
        }
    }
}