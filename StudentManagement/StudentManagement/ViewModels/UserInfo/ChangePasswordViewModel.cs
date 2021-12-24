using StudentManagement.Commands;
using StudentManagement.Services;
using StudentManagement.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly ErrorBaseViewModel _errorBaseViewModel;

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Password))
                {
                    _errorBaseViewModel.AddError(nameof(Password), "Vui lòng nhập mật khẩu!");
                }
                OnPropertyChanged();
            }
        }
        private string _newPassWord;
        public string NewPassWord
        {
            get => _newPassWord;
            set
            {
                _newPassWord = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(NewPassWord))
                {
                    _errorBaseViewModel.AddError(nameof(NewPassWord), "Vui lòng nhập mật khẩu mới!");
                }

                OnPropertyChanged();
            }
        }
        private string _reNewPassWord;
        public string ReNewPassWord
        {
            get => _reNewPassWord;
            set
            {
                _reNewPassWord = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(ReNewPassWord))
                {
                    _errorBaseViewModel.AddError(nameof(ReNewPassWord), "Vui lòng nhập lại mật khẩu mới!");
                }
                if (IsValid(ReNewPassWord))
                {
                if (!ReNewPassWord.Equals(NewPassWord))
                    {
                        _errorBaseViewModel.AddError(nameof(ReNewPassWord), "Mật khẩu nhập lại không trùng với mật khẩu mới");
                    }
                }
                OnPropertyChanged();
            }
        }

        public bool HasErrors => _errorBaseViewModel.HasErrors;
        public ICommand ConFirmCommand { get => _conFirmCommand; set => _conFirmCommand = value; }

        private ICommand _conFirmCommand;

        public ICommand CancelCommand { get => _cancelCommand; set => _cancelCommand = value; }

        private ICommand _cancelCommand;

        public ChangePasswordViewModel()
        {
            try
            {
                _errorBaseViewModel = new ErrorBaseViewModel();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

                _errorBaseViewModel.ClearAllErrors();
                CancelCommand = new RelayCommand<object>((p) => true, (p) => Cancel());
                ConFirmCommand = new RelayCommand<object>(
                    (p) =>
                    {
                        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ReNewPassWord) || string.IsNullOrEmpty(NewPassWord) || !NewPassWord.Equals(ReNewPassWord))
                            return false;
                        return true;
                    },
                    (p) => ConFirm());
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc đổi mật khẩu", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
          
        }
 
        public void Cancel()
        {
            MainViewModel.Instance.IsOpen = false;
        }

        public void ConFirm()
        {
            try
            {
                if (!SHA256Cryptography.Instance.EncryptString(Password).Equals(LoginServices.CurrentUser.Password))
                {
                    MyMessageBox.Show("Mật khẩu không chính xác", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
                UserServices.Instance.ChangePassWordOfCurrentUser(NewPassWord, LoginServices.CurrentUser);
                MyMessageBox.Show("Cập nhật mật khẩu thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                MainViewModel.Instance.IsOpen = false;
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong cập nhật mật khẩu", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
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
    }
}
