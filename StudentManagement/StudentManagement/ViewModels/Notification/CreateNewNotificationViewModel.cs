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
    public class CreateNewNotificationViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region properties
        public NotificationCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private NotificationCard _currentCard;

        private readonly ErrorBaseViewModel _errorBaseViewModel;
        private string _topic;
        private string _content;
        private string _type;

        public string Topic
        {
            get => _topic;
            set
            {
                _topic = value;
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
            get => _type;
            set
            {
                _type = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Type))
                {
                    _errorBaseViewModel.AddError(nameof(Type), "Vui lòng nhập loại bài đăng!");
                }

                OnPropertyChanged();
            }

        }
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Content))
                {
                    _errorBaseViewModel.AddError(nameof(Content), "Vui lòng nhập nội dung!");
                }
                OnPropertyChanged();
            }

        }

        
        public bool HasErrors => _errorBaseViewModel.HasErrors;
        public bool IsCreateNotification { get => _isCreateNotification; set { _isCreateNotification = value; OnPropertyChanged(); } }
        private bool _isCreateNotification;
        #endregion

        #region Icommand
        public ICommand CreateNotificationCommand { get => _createNotificationCommand; set => _createNotificationCommand = value; }
        private ICommand _createNotificationCommand;
        #endregion
        public CreateNewNotificationViewModel(NotificationCard card)
        {
            try
            {
                CurrentCard = card;
                InitCommand();
                IsCreateNotification = false;
                _errorBaseViewModel = new ErrorBaseViewModel();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong khởi tạo thông báo", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            }
        }
        #region validation
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
        #endregion
        #region Method
        public void InitCommand()
        {
            CreateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CreateNewNotification());
        }
 
        public void CreateNewNotification()
        {
            try
            {
                if (!IsValid(Content) || !IsValid(Topic) || !IsValid(Type))
                {
                    MyMessageBox.Show("Nội dung nhập chưa đầy đủ", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
                var AdminNotificationVM = AdminNotificationViewModel.Instance;
                CurrentCard.Content = Content;
                CurrentCard.Topic = Topic;
                CurrentCard.Type = Type;
                AdminNotificationVM.Cards.Insert(0, CurrentCard);
                if (string.IsNullOrEmpty(AdminNotificationVM.SearchInfo))
                    if (AdminNotificationVM.RealCards.Where(x => x.Id == CurrentCard.Id).Count() == 0)
                        AdminNotificationVM.RealCards.Insert(0, CurrentCard);
                NotificationServices.Instance.AddNotificationByNotificationCard(CurrentCard);
                if (CurrentCard.Type.Contains("Thông báo chung") || CurrentCard.Type.Contains("Thông báo Admin"))
                {
                    AdminNotificationVM.NumCardInBadged += 1;
                    AdminNotificationVM.CardsInBadge.Insert(0, CurrentCard);
                }
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong tạo thông báo mới", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
          
        }
        #endregion
    }
}
