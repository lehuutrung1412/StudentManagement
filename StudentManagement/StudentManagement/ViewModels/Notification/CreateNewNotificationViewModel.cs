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
    public class CreateNewNotificationViewModel : BaseViewModel, INotifyDataErrorInfo
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

        
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public ICommand CreateNotificationCommand { get => _createNotificationCommand; set => _createNotificationCommand = value; }
        private ICommand _createNotificationCommand;

        public bool IsCreateNotification { get => _isCreateNotification; set { _isCreateNotification = value; OnPropertyChanged(); } }
        private bool _isCreateNotification;
        public CreateNewNotificationViewModel(CardNotification card)
        {
            CurrentCard = card;
            InitCommand();
            IsCreateNotification = false;
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
        }
        public void InitCommand()
        {
            CreateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CreateNewNotification());

        }
        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
          
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }
        public void CreateNewNotification()
        {
            if (!IsValid(NoiDung) || !IsValid(ChuDe) || !IsValid(LoaiBaiDang))
            {
                MyMessageBox.Show("Noi dung nhập chưa đầy đủ","Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            CurrentCard.NoiDung = NoiDung;
            CurrentCard.ChuDe = ChuDe;
            CurrentCard.LoaiBaiDang = LoaiBaiDang;
            AdminNotificationVM.Cards.Insert(0, CurrentCard);
            if (string.IsNullOrEmpty(AdminNotificationVM.SearchInfo))
                if (AdminNotificationVM.RealCards.Where(x => x.Id == CurrentCard.Id).Count() == 0)
                    AdminNotificationVM.RealCards.Insert(0, CurrentCard);
            AdminNotificationVM.NumCardInBadged += 1;
        }
    }
}
