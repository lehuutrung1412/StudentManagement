using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.UserInfoItemViewModel;
using static StudentManagement.ViewModels.UserInfoViewModel;

namespace StudentManagement.ViewModels
{
    public class EditInfoItemViewModel: BaseViewModel, INotifyDataErrorInfo
    {
        private InfoItem _currendInfoItem;
        public InfoItem CurrendInfoItem { get => _currendInfoItem; set => _currendInfoItem = value; }

        private InfoItem _displayInfoItem;
        public InfoItem DisplayInfoItem { get => _displayInfoItem; set => _displayInfoItem = value; }
        private ObservableCollection<ItemInCombobox> _listItemInCombobox;
        public ObservableCollection<ItemInCombobox> ListItemInCombobox { get => _listItemInCombobox; set { _listItemInCombobox = value; OnPropertyChanged(); } }

        private readonly ErrorBaseViewModel _errorBaseViewModel;
        public string LabelName
        {
            get => _labelName;
            set
            {
                _labelName = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(LabelName))
                {
                    _errorBaseViewModel.AddError(nameof(LabelName), "Vui lòng nhập tên thông tin!");
                }

                OnPropertyChanged();
            }
        }
        private string _labelName;

        public string TypeControl
        {
            get => _typeControl;
            set
            {
                _typeControl = value;
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(TypeControl))
                {
                    _errorBaseViewModel.AddError(nameof(TypeControl), "Vui lòng chọn loại thông tin!");
                }

                OnPropertyChanged();
                OnPropertyChanged();
            }
        }
        private string _typeControl;
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public bool IsEnable { get => _isEnable; set { _isEnable = value; OnPropertyChanged(); } }
        private bool _isEnable;

        public ICommand DeleteInfoItemCommand { get => _deleteInfoItemCommand; set => _deleteInfoItemCommand = value; }

        private ICommand _deleteInfoItemCommand;

        public ICommand DeleteItemCommand { get => _deleteItemCommand; set => _deleteItemCommand = value; }

        private ICommand _deleteItemCommand;
        public ICommand AddItemCommand { get => _addItemCommand; set => _addItemCommand = value; }

        private ICommand _addItemCommand;

        public ICommand UpdateInfoItemCommand { get => _updateInfoItemCommand; set => _updateInfoItemCommand = value; }

        private ICommand _updateInfoItemCommand;

        public EditInfoItemViewModel(InfoItem infoItem)
        {
            try
            {
                _errorBaseViewModel = new ErrorBaseViewModel();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
                CurrendInfoItem = infoItem;
                DisplayInfoItem = new InfoItem(infoItem);
                ListItemInCombobox = new ObservableCollection<ItemInCombobox>();
                if (infoItem.Type == 2)
                    ConvertItemSourceToListItemCombobox();
                ConvertTypeToTypeControl();

                LabelName = CurrendInfoItem.LabelName;

                AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddItem());
                DeleteItemCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => DeleteItem(p));
                UpdateInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateInfoItem());
                DeleteInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteInfoItem());
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc khởi tạo trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }
        public void DeleteInfoItem()
        {
            try
            {
                if (MyMessageBox.Show("Bạn có chắc xoá trường thông tin này: ", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.Yes)
                    return;
                InfoItemServices.Instance.HiddenUserRole_UserInfo(CurrendInfoItem, SettingUserInfoViewModel.Instance.Role);
                UserInfoViewModel.Instance.LoadInfoSource();
                SettingUserInfoViewModel.Instance.GetInfoSourceInSettingByRole();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc xoá trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
          
        }
        public void UpdateInfoItem()
        {
            try
            {
                DisplayInfoItem.ItemSource = new ObservableCollection<string>();
                if (TypeControl == "Combobox")
                {
                    ListItemInCombobox.Where(x => !string.IsNullOrEmpty(x.Value)).ToList().ForEach(s => DisplayInfoItem.ItemSource.Add(s.Value));
                    DisplayInfoItem.Type = 2;
                }
                else if (TypeControl == "Datepicker")
                {
                    DisplayInfoItem.Type = 1;
                }
                else
                    DisplayInfoItem.Type = 0;
                DisplayInfoItem.LabelName = LabelName;
                InfoItemServices.Instance.UpdateUserRole_UserInfoByInfoItem(DisplayInfoItem);
                UserInfoViewModel.Instance.LoadInfoSource();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc cập nhật trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
      

            //for (int i =0; i<UserInfoViewModel.Instance.InfoSource.Count; i++)
            //{
            //    if (UserInfoViewModel.Instance.InfoSource[i].LabelName == CurrendInfoItem.LabelName)
            //    {
            //        UserInfoViewModel.Instance.InfoSource[i] = DisplayInfoItem;
            //        break;
            //    }    
                    
            //}            
            //UserInfoViewModel.Instance.IsOpen = false;
        }

        public void DeleteItem(TextBox p)
        {
            if (p.DataContext == null)
                return;
            var item = p.DataContext as ItemInCombobox;
            ListItemInCombobox.Remove(item);
        }
        public void AddItem()
        {
            ListItemInCombobox.Add(new ItemInCombobox { Id = Guid.NewGuid(), Value = "" });
        }
        public void ConvertItemSourceToListItemCombobox()
        {
            DisplayInfoItem.ItemSource.ToList().ForEach(item=> ListItemInCombobox.Add(new ItemInCombobox() { Value = item , Id = Guid.NewGuid()}));
        }
        public void ConvertTypeToTypeControl()
        {
            switch(DisplayInfoItem.Type)
            {
                case 0:
                    {
                        TypeControl = "Textbox";
                        break;
                    }
                case 1:
                    {
                        TypeControl = "Datepicker";
                        break;
                    }
                case 2:
                    {
                        TypeControl = "Combobox";
                        break;
                    }
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
