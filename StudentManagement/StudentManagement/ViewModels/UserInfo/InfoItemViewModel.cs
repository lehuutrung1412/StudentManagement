using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels.UserInfo
{
    public class InfoItemViewModel: BaseViewModel, INotifyDataErrorInfo
    {
        private readonly ErrorBaseViewModel _errorBaseViewModel;


        private InfoItem _currendInfoItem;
        public InfoItem CurrendInfoItem { get => _currendInfoItem; set => _currendInfoItem = value; }
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Content))
                {
                    _errorBaseViewModel.AddError(nameof(Content), "Vui lòng nhập tên thông tin!");
                }
                if (CurrendInfoItem.LabelName.Contains("Username"))
                {
                    if (UserServices.Instance.FindUserByUsername(Content) != null)
                    {
                        _errorBaseViewModel.AddError(nameof(Content), "Username đã được sử dụng");
                    }
                }
                if(CurrendInfoItem.LabelName.Contains("Địa chỉ email"))
                {
                    if(!IsValidEmail(Content))
                    {
                        _errorBaseViewModel.AddError(nameof(Content), "Địa chỉ email không đúng định dạng!");
                    }
                    if(UserServices.Instance.IsUsedEmail(Content))
                    {
                        _errorBaseViewModel.AddError(nameof(Content), "Địa chỉ email đã được sử dụng ở tài khoản khác!");
                    }    
                }
                if (CurrendInfoItem.LabelName.Contains("Số điện thoại"))
                {
                    if (!Int64.TryParse(Content, out var tmp))
                    {
                        _errorBaseViewModel.AddError(nameof(Content), "Số điện thoại phải là số!");
                    }
                    else
                    {
                        if (Content.Length != 10)
                        {
                            _errorBaseViewModel.AddError(nameof(Content), "Số điện thoại phải đủ 10 số!");
                        }
                    }    
                }
                if (CurrendInfoItem.Type==1)
                {
                    if(!CanConvertDateTime(Content))
                    {
                        _errorBaseViewModel.AddError(nameof(Content), "Ngày nhập không đúng định dạng dd/MM/yyyy!");
                    }
                }    

                OnPropertyChanged();
            }
        }
        private string _content;
        public bool HasErrors => _errorBaseViewModel.HasErrors;
        public InfoItemViewModel(InfoItem infoItem)
        {
            try
            {
                _errorBaseViewModel = new ErrorBaseViewModel();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
                CurrendInfoItem = infoItem;
                if (infoItem.Value != null)
                    Content = infoItem.Value.ToString();
                _errorBaseViewModel.ClearAllErrors();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc khởi tạo trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }    
        }
        public bool CanConvertDateTime(string value)
        {
            return DateTime.TryParse(value, out var tmp);
        }
        public void UpdateValue()
        {
            CurrendInfoItem.Value = Content;
        }
        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }
        bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null)
                return null;
            return _errorBaseViewModel.GetErrors(propertyName);
        }
    }
}
