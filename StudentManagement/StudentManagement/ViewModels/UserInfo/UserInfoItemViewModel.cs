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
using static StudentManagement.ViewModels.UserInfoViewModel;

namespace StudentManagement.ViewModels
{
    public class UserInfoItemViewModel: BaseViewModel, INotifyDataErrorInfo
    {
        public class ItemInCombobox
        {
            public string Value { get; set; }
            public Guid Id { get; set; }
        }
        private ObservableCollection<ItemInCombobox> _listItemInCombobox;
        public ObservableCollection<ItemInCombobox> ListItemInCombobox { get => _listItemInCombobox; set { _listItemInCombobox = value; OnPropertyChanged(); } }

        private InfoItem _currendInfo;

        public InfoItem CurrendInfo { get => _currendInfo; set => _currendInfo = value; }

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

        public bool TypeStudent { get => _typeStudent; set { _typeStudent = value; OnPropertyChanged(); } }       
        private bool _typeStudent;
        public bool TypeLecturer { get => _typeLecturer; set { _typeLecturer = value; OnPropertyChanged(); } }
        private bool _typeLecturer;
        public bool TypeAdmin { get => _typeAdmin; set { _typeAdmin = value; OnPropertyChanged(); } }
        private bool _typeAdmin;

        public bool HasErrors => _errorBaseViewModel.HasErrors;



        public ICommand DeleteItemCommand { get => _deleteItemCommand; set => _deleteItemCommand = value; }
        
        private ICommand _deleteItemCommand;
        public ICommand AddItemCommand { get => _addItemCommand; set => _addItemCommand = value; }

        private ICommand _addItemCommand;

        public ICommand AddInfoItemCommand { get => _addInfoItemCommand; set => _addInfoItemCommand = value; }

        private ICommand _addInfoItemCommand;


        public UserInfoItemViewModel()
        {
            try
            {
                CurrendInfo = new InfoItem();
                CurrendInfo.ItemSource = new ObservableCollection<string>();
                ListItemInCombobox = new ObservableCollection<ItemInCombobox>()
                {
                    new ItemInCombobox { Id = Guid.NewGuid(), Value = "" },
                    new ItemInCombobox { Id = Guid.NewGuid(), Value = "" },
                    new ItemInCombobox { Id = Guid.NewGuid(), Value = "" }
                };
                TypeStudent = false;
                TypeAdmin = false;
                TypeLecturer = false;
                _errorBaseViewModel = new ErrorBaseViewModel();
                _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
                AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddItem());
                DeleteItemCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => DeleteItem(p));
                AddInfoItemCommand = new RelayCommand<object>((p) =>
                {
                    if ((!TypeStudent && !TypeLecturer && !TypeAdmin) || string.IsNullOrEmpty(TypeControl) || string.IsNullOrEmpty(LabelName) || IsHollowComboboxItem())
                        return false;
                    return true;
                },
                (p) => AddInfoItem());
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc khởi tạo trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }
        public bool IsHollowComboboxItem()
        {
            if(TypeControl == "Combobox" && ListItemInCombobox.Where(item => !string.IsNullOrEmpty(item.Value)).Count() == 0)
                return true;
            else
                return false;
        }
        public void AddInfoItem()
        {
            try
            {
                if (TypeControl == "Combobox")
                {
                    ListItemInCombobox.Where(x => !string.IsNullOrEmpty(x.Value)).ToList().ForEach(s => CurrendInfo.ItemSource.Add(s.Value));
                    CurrendInfo.Type = 2;
                }
                else if (TypeControl == "Datepicker")
                {
                    CurrendInfo.Type = 1;
                }
                else
                    CurrendInfo.Type = 0;
                CurrendInfo.LabelName = LabelName;
                //InfoItemServices.Instance.AddUserInfoByInfoItem(CurrendInfo);

                if (TypeStudent)
                {
                    if (!InfoItemServices.Instance.AddUserRole_UserInfoByRoleAndInfoItem(CurrendInfo, "Sinh viên"))
                        MyMessageBox.Show("Chưa tồn tại sinh viên trong hệ thống!!!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                if (TypeLecturer)
                {
                    if (!InfoItemServices.Instance.AddUserRole_UserInfoByRoleAndInfoItem(CurrendInfo, "Giáo viên"))
                        MyMessageBox.Show("Chưa tồn tại giáo viên trong hệ thống!!!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                if (TypeAdmin)
                {
                    if (!InfoItemServices.Instance.AddUserRole_UserInfoByRoleAndInfoItem(CurrendInfo, "Admin"))
                        MyMessageBox.Show("Chưa tồn tại Admin trong hệ thống!!!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                MainViewModel.Instance.IsOpen = false;
                UserInfoViewModel.Instance.LoadInfoSource();
                SettingUserInfoViewModel.Instance.GetInfoSourceInSettingByRole();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc thêm trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
         
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
