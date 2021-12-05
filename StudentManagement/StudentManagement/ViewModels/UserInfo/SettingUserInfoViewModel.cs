using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.UserInfoViewModel;

namespace StudentManagement.ViewModels
{
    public class SettingUserInfoViewModel : BaseViewModel
    {
        public class InfoItemWithViewMode
        {
            private InfoItem _infoItem;
            private EditInfoItemViewModel _editInfoItemViewModel;

            public InfoItem InfoItem { get => _infoItem; set => _infoItem = value; }
            public EditInfoItemViewModel EditInfoItemViewModel { get => _editInfoItemViewModel; set => _editInfoItemViewModel = value; }

            public InfoItemWithViewMode(InfoItem infoItem)
            {
                InfoItem = infoItem;
                EditInfoItemViewModel = new EditInfoItemViewModel(infoItem);
            }
        }
        public class DeleteInfoItemWithViewMode
        {
            private InfoItem _infoItem;
            private DeleteInfoItemViewModel _deleteInfoItemWithViewModel;

            public InfoItem InfoItem { get => _infoItem; set => _infoItem = value; }
            public DeleteInfoItemViewModel DeleteInfoItemWithViewModel { get => _deleteInfoItemWithViewModel; set => _deleteInfoItemWithViewModel = value; }

            public DeleteInfoItemWithViewMode(InfoItem infoItem)
            {
                InfoItem = infoItem;
                DeleteInfoItemWithViewModel = new DeleteInfoItemViewModel(infoItem);
            }
        }

        private static SettingUserInfoViewModel s_instance;
        public static SettingUserInfoViewModel Instance
        {
            get => s_instance ?? (s_instance = new SettingUserInfoViewModel());

            private set => s_instance = value;
        }

        private ObservableCollection<InfoItemWithViewMode> _infoSource;
        public ObservableCollection<InfoItemWithViewMode> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private ObservableCollection<DeleteInfoItemWithViewMode> _deleteSource;
        public ObservableCollection<DeleteInfoItemWithViewMode> DeleteSource { get => _deleteSource; set { _deleteSource = value; OnPropertyChanged(); } }


        private List<bool> _listCheck;
        public List<bool> ListCheck { get => _listCheck; set { _listCheck = value; OnPropertyChanged(); } }

        private string _role;
        public string Role { get => _role; set { _role = value; OnPropertyChanged(); } }

        private object _addNewInfoItem;
        public object AddNewInfoItem { get => _addNewInfoItem; set { _addNewInfoItem = value; OnPropertyChanged(); } }

        public ICommand ConfirmSettingCommand { get => _confirmSettingCommand; set => _confirmSettingCommand = value; }
        private ICommand _confirmSettingCommand;

        public ICommand AddNewInfoItemCommand { get => _addNewInfoItemCommand; set => _addNewInfoItemCommand = value; }
        private ICommand _addNewInfoItemCommand;

        public ICommand GetInfoSourceInSettingByRoleCommand { get => _getInfoSourceInSettingByRoleCommand; set => _getInfoSourceInSettingByRoleCommand = value; }
        private ICommand _getInfoSourceInSettingByRoleCommand;

        private object _isOpen;
        public object IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
        }
        private object _isSetting;
        public object IsSetting
        {
            get { return _isSetting; }
            set
            {
                _isSetting = value;
                OnPropertyChanged();
            }
        }


        public SettingUserInfoViewModel()
        {
            Instance = this;
            AddNewInfoItem = new UserInfoItemViewModel();
            IsOpen = false;
            IsSetting = false;
            InitCommand();
            ResetListCheck();
        }
        public void ResetListCheck()
        {
            ListCheck = new List<bool> { false, false, false };
        }
        public void InitCommand()
        {
            ConfirmSettingCommand = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmSetting());
            AddNewInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddNewInfoItemInSetting());
            GetInfoSourceInSettingByRoleCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetInfoSourceInSettingByRole());
        }
        public void GetInfoSourceInSettingByRole()
        {
            InfoSource = new ObservableCollection<InfoItemWithViewMode>();
            if (ListCheck[0])
            {
                Role = "Học sinh";
                InfoItemServices.Instance.GetInfoSourceInSettingByRole("Học sinh").ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
            }
            else if (ListCheck[1])
            {
                Role = "Giáo viên";
                InfoItemServices.Instance.GetInfoSourceInSettingByRole("Giáo viên").ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
            }
            else
            {
                Role = "Admin";
                InfoItemServices.Instance.GetInfoSourceInSettingByRole("Admin").ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
            }
            GetDeleteSourceInSettingByRole();
            IsSetting = true;
        }
        public void GetDeleteSourceInSettingByRole()
        {
            DeleteSource = new ObservableCollection<DeleteInfoItemWithViewMode>();
            if (ListCheck[0])
                InfoItemServices.Instance.GetDeleteInfoSourceByRole("Học sinh").ToList().ForEach(infoSource => DeleteSource.Add(new DeleteInfoItemWithViewMode(infoSource)));
            else if (ListCheck[1])
                InfoItemServices.Instance.GetDeleteInfoSourceByRole("Giáo viên").ToList().ForEach(infoSource => DeleteSource.Add(new DeleteInfoItemWithViewMode(infoSource)));
            else
                InfoItemServices.Instance.GetDeleteInfoSourceByRole("Admin").ToList().ForEach(infoSource => DeleteSource.Add(new DeleteInfoItemWithViewMode(infoSource)));
        }
        public void AddNewInfoItemInSetting()
        {
            AddNewInfoItem = new UserInfoItemViewModel();
            IsOpen = true;
        }
        public void ReloadSettingViewModel()
        {
            //InfoSource = new ObservableCollection<InfoItemWithViewMode>();
            //UserInfoViewModel.Instance.InfoSource.ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
            IsSetting = false;
            ListCheck = new List<bool> { false, false, false };
        }

        public void ConfirmSetting()
        {
            if (MyMessageBox.Show("Bạn muốn lưu cài đặt này", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
            {
                MyMessageBox.Show("Cài đặt thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                InfoSource.ToList().ForEach(infoSource => infoSource.EditInfoItemViewModel.UpdateInfoItem());
            }
            GetInfoSourceInSettingByRole();
            return;
        }
    }
}
