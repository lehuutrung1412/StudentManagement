using StudentManagement.Commands;
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
    public class ShowDetailNotificationViewModel: BaseViewModel, INotifyDataErrorInfo
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        private readonly ErrorBaseViewModel _errorBaseViewModel;
        private string _chuDe;
        private string _noiDung;
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
        public string NoiDung
        {
            get => _noiDung;
            set
            {
                _noiDung = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(NoiDung))
                {
                    _errorBaseViewModel.AddError(nameof(NoiDung), "Vui lòng nhập nội dung!");
                }
                OnPropertyChanged();
            }

        }

        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public ICommand IsEditNotificationCommand { get => _isEditNotificationCommand; set => _isEditNotificationCommand = value; }
        private ICommand _isEditNotificationCommand;
        public ICommand CancelEditCommand { get => _cancelEditCommand; set => _cancelEditCommand = value; }
        private ICommand _cancelEditCommand;
        public ICommand EditNotificationCommand { get => _editNotificationCommand; set => _editNotificationCommand = value; }
        private ICommand _editNotificationCommand;
        public ICommand DeleteNotificationCommand { get => _deleteNotification; set => _deleteNotification = value; }
        private ICommand _deleteNotification;

        public bool IsEnable { get => _isEnable; set { _isEnable = value; OnPropertyChanged(); } }

        private bool _isEnable;

        public ShowDetailNotificationViewModel(CardNotification card)
        {
            this.CurrentCard = card;
            IsEnable = false;
            _errorBaseViewModel = new ErrorBaseViewModel();
            ChuDe = CurrentCard.ChuDe;
            NoiDung = CurrentCard.NoiDung;
            LoaiBaiDang = CurrentCard.LoaiBaiDang;
            InitCommand();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
        }
        public void InitCommand()
        {
            IsEditNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => IsEditNotification());
            EditNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => EditNotification());
            CancelEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelEdit());
            DeleteNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteNotification());
        }

        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }

        public void DeleteNotification()
        {
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            AdminNotificationVM.Cards.Remove(CurrentCard);
            AdminNotificationVM.RealCards.Remove(CurrentCard);
        }
        public void CancelEdit()
        {
            IsEnable = false;
        }
        public void IsEditNotification()
        {
            IsEnable = true;
        }
        public void EditNotification()
        {
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            CurrentCard.ChuDe = ChuDe;
            CurrentCard.NoiDung = NoiDung;
            for(int i=0; i<AdminNotificationVM.Cards.Count;i++)
            {
                if (AdminNotificationVM.Cards[i].Id == CurrentCard.Id)
                {
                    AdminNotificationVM.Cards[i] = CurrentCard;
                    break;
                }    
            }
            for (int i = 0; i < AdminNotificationVM.RealCards.Count; i++)
            {
                if (AdminNotificationVM.RealCards[i].Id == CurrentCard.Id)
                {
                    AdminNotificationVM.RealCards[i] = CurrentCard;
                    break;
                }
            }
            IsEnable = false;
        }
    }
}