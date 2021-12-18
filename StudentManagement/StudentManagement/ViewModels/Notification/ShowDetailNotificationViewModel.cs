using StudentManagement.Commands;
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
    public class ShowDetailNotificationViewModel : BaseViewModel, INotifyDataErrorInfo
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

        public bool CanUpdate => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;
        public bool IsEnable { get => _isEnable; set { _isEnable = value; OnPropertyChanged(); } }

        private bool _isEnable;
        #endregion
        #region Icommand
        public ICommand IsEditNotificationCommand { get => _isEditNotificationCommand; set => _isEditNotificationCommand = value; }
        private ICommand _isEditNotificationCommand;
        public ICommand CancelEditCommand { get => _cancelEditCommand; set => _cancelEditCommand = value; }
        private ICommand _cancelEditCommand;
        public ICommand EditNotificationCommand { get => _editNotificationCommand; set => _editNotificationCommand = value; }
        private ICommand _editNotificationCommand;
        public ICommand DeleteNotificationCommand { get => _deleteNotification; set => _deleteNotification = value; }
        private ICommand _deleteNotification;
        #endregion


        public ShowDetailNotificationViewModel(NotificationCard card)
        {
            try
            {
                CurrentCard = card;
                IsEnable = false;
                _errorBaseViewModel = new ErrorBaseViewModel();
                Topic = CurrentCard.Topic;
                Content = CurrentCard.Content;
                Type = CurrentCard.Type;
                InitCommand();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
                MainViewModel.Instance.LayoutViewModel.ContentViewModel = AdminNotificationViewModel.Instance;
                MainViewModel.Instance.LayoutViewModel.RightSideBar = AdminNotificationRightSideBarViewModel.Instance;
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong khởi tạo thông tin cá nhân", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
            OnPropertyChanged(nameof(CanUpdate));
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
            IsEditNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => IsEditNotification());
            EditNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => EditNotification());
            CancelEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelEdit());
            DeleteNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteNotification());
        }

 

        public void DeleteNotification()
        {
            try
            {
                var AdminNotificationVM = Instance;
                AdminNotificationVM.Cards.Remove(CurrentCard);
                AdminNotificationVM.RealCards.Remove(CurrentCard);
                AdminNotificationVM.CardsInBadge.Remove(CurrentCard);
                if (UserServices.Instance.GetUserById(AdminNotificationVM.IdUser).UserRole.Role.Contains("Admin"))
                    NotificationServices.Instance.DeleteNotificationByNotificationCard(CurrentCard);
                else
                    NotificationServices.Instance.DeleteNotificationInfoByNotificationCardAndIdUser(CurrentCard, LoginServices.CurrentUser.Id);
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc xoá thông báo", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        
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
            try
            {
                var AdminNotificationVM = Instance;
                CurrentCard.Topic = Topic;
                CurrentCard.Content = Content;
                for (int i = 0; i < AdminNotificationVM.Cards.Count; i++)
                {
                    if (AdminNotificationVM.Cards[i].Id == CurrentCard.Id)
                    {
                        AdminNotificationVM.Cards.Remove(AdminNotificationVM.Cards[i]);
                        AdminNotificationVM.Cards.Insert(i, CurrentCard);

                        break;
                    }
                }
                for (int i = 0; i < AdminNotificationVM.RealCards.Count; i++)
                {
                    if (AdminNotificationVM.RealCards[i].Id == CurrentCard.Id)
                    {

                        AdminNotificationVM.RealCards.Remove(AdminNotificationVM.RealCards[i]);
                        AdminNotificationVM.RealCards.Insert(i, CurrentCard);
                        break;
                    }
                }
                for (int i = 0; i < AdminNotificationVM.CardsInBadge.Count; i++)
                    if (AdminNotificationVM.CardsInBadge[i].Id == CurrentCard.Id)
                    {
                        AdminNotificationVM.CardsInBadge.Remove(AdminNotificationVM.CardsInBadge[i]);
                        AdminNotificationVM.CardsInBadge.Insert(i, CurrentCard);
                        break;
                    }
                NotificationServices.Instance.UpdateNotificationByNotificationCard(CurrentCard);
                IsEnable = false;
            }    
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong cập nhật thông báo", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        #endregion
    }
}