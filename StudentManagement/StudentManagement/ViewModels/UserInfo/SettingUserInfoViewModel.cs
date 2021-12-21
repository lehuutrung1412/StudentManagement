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
        //public class InfoItemWithViewMode
        //{
        //    private InfoItem _infoItem;
        //    private EditInfoItemViewModel _editInfoItemViewModel;

        //    public InfoItem InfoItem { get => _infoItem; set => _infoItem = value; }
        //    public EditInfoItemViewModel EditInfoItemViewModel { get => _editInfoItemViewModel; set => _editInfoItemViewModel = value; }

        //    public InfoItemWithViewMode(InfoItem infoItem)
        //    {
        //        InfoItem = infoItem;
        //        EditInfoItemViewModel = new EditInfoItemViewModel(infoItem);
        //    }
        //}
        //public class DeleteInfoItemWithViewMode
        //{
        //    private InfoItem _infoItem;
        //    private DeleteInfoItemViewModel _deleteInfoItemWithViewModel;

        //    public InfoItem InfoItem { get => _infoItem; set => _infoItem = value; }
        //    public DeleteInfoItemViewModel DeleteInfoItemWithViewModel { get => _deleteInfoItemWithViewModel; set => _deleteInfoItemWithViewModel = value; }

        //    public DeleteInfoItemWithViewMode(InfoItem infoItem)
        //    {
        //        InfoItem = infoItem;
        //        DeleteInfoItemWithViewModel = new DeleteInfoItemViewModel(infoItem);
        //    }
        //}

        private static SettingUserInfoViewModel s_instance;
        public static SettingUserInfoViewModel Instance
        {
            get => s_instance ?? (s_instance = new SettingUserInfoViewModel());

            private set => s_instance = value;
        }

        private ObservableCollection<EditInfoItemViewModel> _infoSource;
        public ObservableCollection<EditInfoItemViewModel> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private ObservableCollection<DeleteInfoItemViewModel> _deleteSource;
        public ObservableCollection<DeleteInfoItemViewModel> DeleteSource { get => _deleteSource; set { _deleteSource = value; OnPropertyChanged(); } }


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
            try
            {
                Instance = this;
                AddNewInfoItem = new UserInfoItemViewModel();
                IsSetting = false;
                InitCommand();
                ResetListCheck();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc khởi tạo trường thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
     
        }
        public void ResetListCheck()
        {
            ListCheck = new List<bool> { false, false, false };
        }
        public void InitCommand()
        {
            ConfirmSettingCommand = new RelayCommand<object>(
                (p) => 
                {
                    if (InfoSource == null)
                        return true;
                    foreach (var infoItem in InfoSource)
                        if (infoItem.HasErrors)
                            return false;
                    return true;
                }, 
                (p) => ConfirmSetting());
            AddNewInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddNewInfoItemInSetting());
            GetInfoSourceInSettingByRoleCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetInfoSourceInSettingByRole());
        }
        public void GetInfoSourceInSettingByRole()
        {
            try
            {
                InfoSource = new ObservableCollection<EditInfoItemViewModel>();
                DeleteSource = new ObservableCollection<DeleteInfoItemViewModel>();
                if (ListCheck[0])
                {
                    Role = "Sinh viên";
                    InfoItemServices.Instance.GetInfoSourceInSettingByRole("Sinh viên").ToList().ForEach(infoSource => InfoSource.Add(new EditInfoItemViewModel(infoSource)));
                    InfoItemServices.Instance.GetDeleteInfoSourceByRole("Sinh viên").ToList().ForEach(infoSource => DeleteSource.Add(new DeleteInfoItemViewModel(infoSource)));
                }
                else if (ListCheck[1])
                {
                    Role = "Giáo viên";
                    InfoItemServices.Instance.GetInfoSourceInSettingByRole("Giáo viên").ToList().ForEach(infoSource => InfoSource.Add(new EditInfoItemViewModel(infoSource)));
                    InfoItemServices.Instance.GetDeleteInfoSourceByRole("Giáo viên").ToList().ForEach(infoSource => DeleteSource.Add(new DeleteInfoItemViewModel(infoSource)));
                }
                else
                {
                    Role = "Admin";
                    InfoItemServices.Instance.GetInfoSourceInSettingByRole("Admin").ToList().ForEach(infoSource => InfoSource.Add(new EditInfoItemViewModel(infoSource)));
                    InfoItemServices.Instance.GetDeleteInfoSourceByRole("Admin").ToList().ForEach(infoSource => DeleteSource.Add(new DeleteInfoItemViewModel(infoSource)));
                }
                IsSetting = true;
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc khởi tạo trường thông tin theo vai trò", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        
        }
        public void AddNewInfoItemInSetting()
        {
            AddNewInfoItem = new UserInfoItemViewModel();
            MainViewModel.Instance.DialogViewModel = AddNewInfoItem;
            MainViewModel.Instance.IsOpen = true;
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
            try
            {
                if (MyMessageBox.Show("Bạn muốn lưu cài đặt này", "Thông báo", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
                {
                    MyMessageBox.Show("Cài đặt thành công", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    InfoSource.ToList().ForEach(infoSource => infoSource.UpdateInfoItem());
                }
                GetInfoSourceInSettingByRole();
                return;
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong việc cập nhật cài đặt", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
           
        }
    }
}
