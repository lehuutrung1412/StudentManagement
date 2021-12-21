using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Forms;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Models;
using StudentManagement.ViewModels.UserInfo;

namespace StudentManagement.ViewModels
{
    public class UserInfoViewModel : BaseViewModel
    {
        private static UserInfoViewModel s_instance;
        public static UserInfoViewModel Instance
        {
            get => s_instance ?? (s_instance = new UserInfoViewModel());

            private set => s_instance = value;
        }

        private ObservableCollection<InfoItemViewModel> _infoSource;

        public ObservableCollection<InfoItemViewModel> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private List<InfoItem> _displaySource;

        public List<InfoItem> DisplaySource { get => _displaySource; set { _displaySource = value; OnPropertyChanged(); } }

        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(); } }
        private string _avatar;

        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private string _displayName;

        public string Role { get => _role; set { _role = value; OnPropertyChanged(); } }
        private string _role;

        public int NumPeriodToday { get => _numPeriodToday; set { _numPeriodToday = value; OnPropertyChanged(); } }
        private int _numPeriodToday;

        public ObservableCollection<string> ListTypeControl { get => _listTypeControl; set { _listTypeControl = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _listTypeControl;

        public object _userInfoItemViewModel;
        public object _editInfoItemViewModel;
        public object _changePasswordViewModel;

        private object _isUpdate;
        public object IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
            }
        }
        private Guid _idUser;
        public Guid IdUser { get => _idUser; set => _idUser = value; }

        public ICommand ClickChangeImageCommand { get => _clickChangeImageCommand; set => _clickChangeImageCommand = value; }
        private ICommand _clickChangeImageCommand;

        public ICommand AddNewInfoItemCommand { get => _addNewInfoItemCommand; set => _addNewInfoItemCommand = value; }
        private ICommand _addNewInfoItemCommand;

        public ICommand UpdateUserInfoCommand { get => _updateUserInfoCommand; set => _updateUserInfoCommand = value; }
        private ICommand _updateUserInfoCommand;

        public ICommand ConfirmUserInfoCommand { get => _confirmUserInfoCommand; set => _confirmUserInfoCommand = value; }
        private ICommand _confirmUserInfoCommand;
        public ICommand CancelUpdateCommand { get => _cancelUpdateCommand; set => _cancelUpdateCommand = value; }
        private ICommand _cancelUpdateCommand;

        public ICommand ChangePasswordCommand { get => _changePasswordCommand; set => _changePasswordCommand = value; }
        private ICommand _changePasswordCommand;

        public UserInfoViewModel()
        {
            Instance = this;
            LoginServices.UpdateCurrentUser += LoginServices_UpdateCurrentUser;

            //ObservableCollection<string> Faculty = new ObservableCollection<string> { "KHMT", "KTPM" };
            //ObservableCollection<string> Sex = new ObservableCollection<string> { "Nam", "Nữ" };
            //ObservableCollection<string> Class = new ObservableCollection<string> { "KHMT2019", "KHTN2019" };
            //ObservableCollection<string> TrainingForm = new ObservableCollection<string> { "CNTN", "CQDT" };
            //InfoSource = new ObservableCollection<InfoItem>()
            //{
            //    new InfoItem(Guid.NewGuid(),"Họ và tên",0,null,"Nguyễn Đỗ Mạnh Cường",false),
            //    new InfoItem(Guid.NewGuid(),"Ngày sinh",1,null,"14/02/2001",false),
            //    new InfoItem(Guid.NewGuid(),"Địa chỉ",0,null,"02B1, chợ mới Ninh Hoà",true),
            //    new InfoItem(Guid.NewGuid(),"Khoa",2,Faculty,"KHMT",false),
            //    new InfoItem(Guid.NewGuid(),"Số điện thoại",0,null,"0937418670",true),
            //    new InfoItem(Guid.NewGuid(),"Giới tính",2,Sex,"Nam",false),
            //    new InfoItem(Guid.NewGuid(),"Địa chỉ mail",0,null,"cuongnguyen14022001",true),
            //    new InfoItem(Guid.NewGuid(),"Hệ",2,TrainingForm,"CNTN",false),
            //    new InfoItem(Guid.NewGuid(),"Lớp sinh hoạt",2,Class,"KHTN2019",false),
            //};
            if (LoginServices.CurrentUser != null)
            {
                IdUser = LoginServices.CurrentUser.Id;
                Avatar = LoginServices.CurrentUser.DatabaseImageTable?.Image;
                DisplayName = LoginServices.CurrentUser?.DisplayName;
                Role = LoginServices.CurrentUser?.UserRole.Role;
                NumPeriodToday = CourseRegisterServices.Instance.CountPeriodTodayOfUser();
                LoadInfoSource();
            }
            ListTypeControl = new ObservableCollection<string> { "Combobox", "Textbox", "Datepicker" };
            IsUpdate = false;
           
            InitCommand();
        }
        public void InitCommand()
        {
            ClickChangeImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickChangeImage());
            AddNewInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddNewInfoItem());
            UpdateUserInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateUserInfo());
            CancelUpdateCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelUpdate());
            ChangePasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) => ChangePassword());
            ConfirmUserInfoCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (InfoSource == null)
                        return false;
                    foreach (var infoItem in InfoSource)
                    {
                        if (infoItem.HasErrors)
                            return false;
                    }
                    return true;
                },
                (p) => ComfirmUserInfo());
        }
        public void ChangePassword()
        {
            _changePasswordViewModel = new ChangePasswordViewModel();
            MainViewModel.Instance.DialogViewModel = _changePasswordViewModel;
            MainViewModel.Instance.IsOpen = true;
        }
        private void LoginServices_UpdateCurrentUser(object sender, LoginServices.LoginEvent e)
        {
            try
            {
                IdUser = LoginServices.CurrentUser.Id;
                Avatar = LoginServices.CurrentUser.DatabaseImageTable?.Image;
                DisplayName = LoginServices.CurrentUser?.DisplayName;
                Role = LoginServices.CurrentUser?.UserRole.Role;
                NumPeriodToday = CourseRegisterServices.Instance.CountPeriodTodayOfUser();
                UserInfoViewModel.Instance.LoadInfoSource();
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong khởi tạo thông tin cá nhân", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }

        public void LoadInfoSource()
        {
            try
            {
                var user = UserServices.Instance.GetUserById(IdUser);
                InfoSource = new ObservableCollection<InfoItemViewModel>()
            {
                new InfoItemViewModel(new InfoItem(Guid.NewGuid(),"Họ và tên",0,null,user.DisplayName,false)),
            };
                if (string.IsNullOrEmpty(user.Email))
                    InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Địa chỉ email", 0, null, user.Email, true)));
                else
                    InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Địa chỉ email", 0, null, user.Email, false)));
                switch (user.UserRole.Role)
                {
                    case "Sinh viên":
                        {
                            var student = StudentServices.Instance.GetStudentbyUser(user);
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), student.Faculty.DisplayName, false)));
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), student.TrainingForm.DisplayName, false)));
                            break;
                        }
                    case "Giáo viên":
                        {
                            var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), lecture.Faculty.DisplayName, false)));
                            break;
                        }
                    case "Admin":
                        {
                            foreach (var infoItemViewModel in InfoSource)
                                infoItemViewModel.CurrendInfoItem.IsEnable = true;
                            break;
                        }
                }
                var listInfoItem = InfoItemServices.Instance.GetInfoSourceByUserId(IdUser);
                foreach (var infoItem in listInfoItem)
                {
                    InfoSource.Add(new InfoItemViewModel(infoItem));
                }
            }
            catch 
            {
                MyMessageBox.Show("Đã có lỗi trong việc tải thông tin cá nhân", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
   
        }

        public void CancelUpdate()
        {
            InfoSource = new ObservableCollection<InfoItemViewModel>();
            DisplaySource.ForEach(info => InfoSource.Add(new InfoItemViewModel(new InfoItem(info))));
            IsUpdate = false;
        }

        public void ComfirmUserInfo()
        {
            try
            {
                if (MyMessageBox.Show("Bạn có muốn cập nhật thông tin", "Thông báo", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Information) != System.Windows.MessageBoxResult.OK)
                {
                    InfoSource = new ObservableCollection<InfoItemViewModel>();
                    DisplaySource.ForEach(info => InfoSource.Add(new InfoItemViewModel(new InfoItem(info))));
                }
                else
                {
                    var user = UserServices.Instance.GetUserById(IdUser);
                    //InfoSource.ToList().ForEach(item => InfoItemServices.Instance.UpdateUser_UserRole_UserInfoByInfoItem(item));
                    foreach (var infoItem in InfoSource)
                    {
                        infoItem.UpdateValue();
                        switch (infoItem.CurrendInfoItem.LabelName)
                        {
                            case "Họ và tên":
                                {
                                    user.DisplayName = infoItem.CurrendInfoItem.Value.ToString();
                                    break;
                                }
                            case "Địa chỉ email":
                                {
                                    user.Email = infoItem.CurrendInfoItem.Value.ToString();
                                    break;
                                }
                            case "Hệ đào tạo":
                                {
                                    var student = StudentServices.Instance.GetStudentbyUser(user);
                                    student.IdTrainingForm = DataProvider.Instance.Database.TrainingForms.FirstOrDefault(trainingForm => trainingForm.DisplayName == infoItem.CurrendInfoItem.Value.ToString()).Id;
                                    break;
                                }
                            case "Khoa":
                                {
                                    switch (user.UserRole.Role)
                                    {
                                        case "Sinh viên":
                                            {
                                                var student = StudentServices.Instance.GetStudentbyUser(user);
                                                student.IdFaculty = DataProvider.Instance.Database.Faculties.FirstOrDefault(faculty => faculty.DisplayName == infoItem.CurrendInfoItem.Value.ToString()).Id;
                                                break;
                                            }
                                        case "Giáo viên":
                                            {
                                                var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                                                lecture.IdFaculty = DataProvider.Instance.Database.Faculties.FirstOrDefault(faculty => faculty.DisplayName == infoItem.CurrendInfoItem.Value.ToString()).Id;
                                                break;
                                            }
                                    }
                                    break;
                                }

                            default:
                                {
                                    InfoItemServices.Instance.UpdateUser_UserRole_UserInfoByInfoItem(infoItem.CurrendInfoItem);
                                    break;
                                }
                        }
                        DataProvider.Instance.Database.SaveChanges();
                    }
                }
            }
            catch 
            {
                MyMessageBox.Show("Đã có lỗi khi cập nhật thông tin!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
       
            IsUpdate = false;
        }
        public void UpdateUserInfo()
        {
            IsUpdate = true;
            DisplaySource = new List<InfoItem>();
            foreach (var info in InfoSource)
            {
                var item = new InfoItem(info.CurrendInfoItem);
                DisplaySource.Add(item);
            }
        }
        public void AddNewInfoItem()
        {
            this._userInfoItemViewModel = new UserInfoItemViewModel();
            MainViewModel.Instance.DialogViewModel = this._userInfoItemViewModel;

            MainViewModel.Instance.IsOpen = true;
        }
        public async void ClickChangeImage()
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                };
                if (op.ShowDialog() == DialogResult.OK)
                {
                    var uploadImageTasks = new List<Task<string>>();
                    uploadImageTasks.Add(ImageUploader.Instance.UploadAsync(op.FileName));
                    foreach (var img in await Task.WhenAll(uploadImageTasks))
                    {
                        await UserServices.Instance.SaveImageToUser(img);
                    }
                    Avatar = op.FileName;
                }
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi khi thay đổi ảnh đại diện", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
         
        }
    }
}
