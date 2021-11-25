using StudentManagement.Commands;
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
                InfoItem = new InfoItem(infoItem);
                EditInfoItemViewModel = new EditInfoItemViewModel(InfoItem);
            }
        }
        private ObservableCollection<InfoItemWithViewMode> _infoSource;
        public ObservableCollection<InfoItemWithViewMode> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        public ICommand ConfirmSettingCommand { get => _confirmSettingCommand; set => _confirmSettingCommand = value; }

        private ICommand _confirmSettingCommand;

        public SettingUserInfoViewModel()
        {
            InfoSource = new ObservableCollection<InfoItemWithViewMode>();
            UserInfoViewModel.Instance.InfoSource.ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
            ConfirmSettingCommand = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmSetting());
        }

        public void ConfirmSetting()
        {
            if (MyMessageBox.Show("Bạn muốn lưu cài đặt này", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
            {
                MyMessageBox.Show("Cài đặt thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                InfoSource.ToList().ForEach(infoSource => infoSource.EditInfoItemViewModel.UpdateInfoItem());  
            }
            InfoSource = new ObservableCollection<InfoItemWithViewMode>();
            UserInfoViewModel.Instance.InfoSource.ToList().ForEach(infoSource => InfoSource.Add(new InfoItemWithViewMode(infoSource)));
            return;
        }
    }
}
