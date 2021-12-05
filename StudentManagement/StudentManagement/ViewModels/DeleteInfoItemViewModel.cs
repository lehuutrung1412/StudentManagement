using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class DeleteInfoItemViewModel: BaseViewModel
    {
        private InfoItem _currendInfoItem;
        public InfoItem CurrendInfoItem { get => _currendInfoItem; set => _currendInfoItem = value; }

        public ICommand DeleteInfoItemCommand { get => _deleteInfoItemCommand; set => _deleteInfoItemCommand = value; }

        private ICommand _deleteInfoItemCommand;

        public ICommand RestoreInfoItemCommand { get => _restoreInfoItemCommand; set => _restoreInfoItemCommand = value; }

        private ICommand _restoreInfoItemCommand;

        public DeleteInfoItemViewModel(InfoItem infoItem)
        {
            CurrendInfoItem = infoItem;
            DeleteInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteInfoItem());
            RestoreInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => RestoreInfoItem());
        }
        public void DeleteInfoItem()
        {
            if (MyMessageBox.Show("Bạn có xoá vĩnh viễn trường thông tin này: ", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.Yes)
                return;
            InfoItemServices.Instance.DeleteUserRole_UserInfo(CurrendInfoItem, SettingUserInfoViewModel.Instance.Role);
            SettingUserInfoViewModel.Instance.GetInfoSourceInSettingByRole();
        }
        public void RestoreInfoItem()
        {
            if (MyMessageBox.Show("Bạn có khôi phục trường thông tin này: ", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.Yes)
                return;
            InfoItemServices.Instance.RestoreUserRole_UserInfo(CurrendInfoItem, SettingUserInfoViewModel.Instance.Role);
            UserInfoViewModel.Instance.LoadInfoSource();
            SettingUserInfoViewModel.Instance.GetInfoSourceInSettingByRole();
        }

    }
}
