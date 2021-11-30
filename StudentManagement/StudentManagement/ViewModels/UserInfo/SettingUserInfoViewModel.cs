using StudentManagement.Commands;
using StudentManagement.Objects;
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

        private static SettingUserInfoViewModel s_instance;
        public static SettingUserInfoViewModel Instance
        {
            get => s_instance ?? (s_instance = new SettingUserInfoViewModel());

            private set => s_instance = value;
        }

        private ObservableCollection<InfoItemWithViewMode> _infoSource;
        public ObservableCollection<InfoItemWithViewMode> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private object _addNewInfoItem;
        public object AddNewInfoItem { get => _addNewInfoItem; set { _addNewInfoItem = value; OnPropertyChanged(); } }

        public ICommand ConfirmSettingCommand { get => _confirmSettingCommand; set => _confirmSettingCommand = value; }
        private ICommand _confirmSettingCommand;

        public ICommand AddNewInfoItemCommand { get => _addNewInfoItemCommand; set => _addNewInfoItemCommand = value; }
        private ICommand _addNewInfoItemCommand;

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

        

        public SettingUserInfoViewModel()
        {
            Instance = this;
            ReloadInfoSource();
            AddNewInfoItem = new UserInfoItemViewModel();
            IsOpen = false; 
            ConfirmSettingCommand = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmSetting());
            AddNewInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddNewInfoItemInSetting());
        }
        public void AddNewInfoItemInSetting()
        {
            AddNewInfoItem = new UserInfoItemViewModel();
            IsOpen = true;
        }
        public void ReloadInfoSource()
        {
            InfoSource = new ObservableCollection<InfoItemWithViewMode>();
            UserInfoViewModel.Instance.InfoSource.ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
        }
     
        public void ConfirmSetting()
        {
            if (MyMessageBox.Show("Bạn muốn lưu cài đặt này", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
            {
                MyMessageBox.Show("Cài đặt thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                InfoSource.ToList().ForEach(infoSource => infoSource.EditInfoItemViewModel.UpdateInfoItem());  
            }
            ReloadInfoSource();
            return;
        }
    }
}
