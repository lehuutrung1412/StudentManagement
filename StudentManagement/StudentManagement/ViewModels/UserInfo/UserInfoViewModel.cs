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

        private ObservableCollection<InfoItem> _infoSource;

        public ObservableCollection<InfoItem> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private List<InfoItem> _displaySource;

        public List<InfoItem> DisplaySource { get => _displaySource; set { _displaySource = value; OnPropertyChanged(); } }

        public bool IsChangeAvatar { get => _isChangeAvatar; set { _isChangeAvatar = value; OnPropertyChanged(); } }

        private bool _isChangeAvatar;

        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(); } }
        private string _avatar;
        public ObservableCollection<string> ListTypeControl { get => _listTypeControl; set { _listTypeControl = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _listTypeControl;      

        public object _userInfoItemViewModel;
        public object _editInfoItemViewModel;

        private object _dialogItemViewModel;
        public object DialogItemViewModel
        {
            get { return _dialogItemViewModel; }
            set
            {
                _dialogItemViewModel = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand ClickImageCommand { get => _clickImageCommand; set => _clickImageCommand = value; }
        private ICommand _clickImageCommand;

        public ICommand ClickChangeImageCommand { get => _clickChangeImageCommand; set => _clickChangeImageCommand = value; }
        private ICommand _clickChangeImageCommand;

        public ICommand AddNewInfoItemCommand { get => _addNewInfoItemCommand; set => _addNewInfoItemCommand = value; }
        private ICommand _addNewInfoItemCommand;

        public ICommand EditInfoItemCommand { get => _editInfoItemCommand; set => _editInfoItemCommand = value; }
        private ICommand _editInfoItemCommand;

        public ICommand UpdateUserInfoCommand { get => _updateUserInfoCommand; set => _updateUserInfoCommand = value; }
        private ICommand _updateUserInfoCommand;

        public ICommand ConfirmUserInfoCommand { get => _confirmUserInfoCommand; set => _confirmUserInfoCommand = value; }
        private ICommand _confirmUserInfoCommand;

        public UserInfoViewModel()
        {
            Instance = this;
            IsChangeAvatar = false;
            Avatar = "https://picsum.photos/200";

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
            IdUser = DataProvider.Instance.Database.Users.FirstOrDefault(user => user.UserRole.Role.Contains("Học sinh")).Id;
            LoadInfoSource();
            ListTypeControl = new ObservableCollection<string> { "Combobox", "Textbox", "Datepicker" };

            IsOpen = false;
            IsUpdate = false;
            ClickImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickImage());
            ClickChangeImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickChangeImage());
            AddNewInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddNewInfoItem());
            EditInfoItemCommand = new RelayCommand<System.Windows.Controls.UserControl>((p) => { return true; }, (p) => EditInfoItem(p));
            UpdateUserInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateUserInfo());
            ConfirmUserInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => ComfirmUserInfo());

        }
        public void LoadInfoSource()
        {
            var user = UserServices.Instance.GetUserById(IdUser);
            InfoSource = new ObservableCollection<InfoItem>()
            {
                new InfoItem(Guid.NewGuid(),"Họ và tên",0,null,user.DisplayName,false),
                new InfoItem(Guid.NewGuid(),"Địa chỉ email",0,null,user.Email,false),
            };
            switch (user.UserRole.Role)
            {
                case "Học sinh":
                    {
                        var student = StudentServices.Instance.GetStudentbyUser(user);
                        InfoSource.Add(new InfoItem(Guid.NewGuid(),"Khoa",2,FacultyServices.Instance.LoadListFaculty(), student.Faculty.DisplayName,true));
                        InfoSource.Add(new InfoItem(Guid.NewGuid(),"Hệ đào tạo",2,TrainingFormServices.Instance.LoadListTrainingForm(),student.TrainingForm.DisplayName,true));
                        break;
                    }
                case "Giáo viên":
                    {
                        var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), lecture.Faculty.DisplayName, false));
                        break;
                    }
                case "Admin":
                    {
                        foreach(var infoItem in InfoSource)
                            infoItem.IsEnable = true;
                        break;
                    }
            }    
            var listInfoItem = InfoItemServices.Instance.GetInfoSourceByUserId(IdUser);
            foreach(var infoItem in listInfoItem)
            {
                InfoSource.Add(infoItem);
            }    
        }
        public void ComfirmUserInfo()
        {
            if (MyMessageBox.Show("Bạn có muốn cập nhật thông tin", "Thông báo", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Information) != System.Windows.MessageBoxResult.OK)
            {
                InfoSource = new ObservableCollection<InfoItem>();
                DisplaySource.ForEach(info => InfoSource.Add(new InfoItem(info)));
            }
            else
            {
                var user = UserServices.Instance.GetUserById(IdUser);
                //InfoSource.ToList().ForEach(item => InfoItemServices.Instance.UpdateUser_UserRole_UserInfoByInfoItem(item));
                foreach (var infoItem in InfoSource)
                {
                    switch (infoItem.LabelName)
                    {
                        case "Họ và tên":
                            {
                                user.DisplayName = infoItem.Value.ToString();
                                break;
                            }
                        case "Địa chỉ email":
                            {
                                user.Email = infoItem.Value.ToString();
                                break;
                            }
                        case "Hệ đào tạo":
                            {
                                var student = StudentServices.Instance.GetStudentbyUser(user);
                                student.IdTrainingForm = DataProvider.Instance.Database.TrainingForms.FirstOrDefault(trainingForm=>trainingForm.DisplayName== infoItem.Value.ToString()).Id;
                                break;
                            }
                        case "Khoa":
                            {
                                switch (user.UserRole.Role)
                                {
                                    case "Học sinh":
                                        {
                                            var student = StudentServices.Instance.GetStudentbyUser(user);
                                            student.IdFaculty = DataProvider.Instance.Database.Faculties.FirstOrDefault(faculty=> faculty.DisplayName== infoItem.Value.ToString()).Id;
                                            break;
                                        }
                                    case "Giáo viên":
                                        {
                                            var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                                            lecture.IdFaculty = DataProvider.Instance.Database.Faculties.FirstOrDefault(faculty => faculty.DisplayName == infoItem.Value.ToString()).Id;
                                            break;
                                        }
                                }
                                break;
                            }

                        default:
                            {
                                InfoItemServices.Instance.UpdateUser_UserRole_UserInfoByInfoItem(infoItem);
                                break;
                            }
                    }
                    DataProvider.Instance.Database.SaveChanges();
                }    
            }
            IsUpdate = false;
        }
        public void UpdateUserInfo()
        {
            IsUpdate = true;
            DisplaySource = new List<InfoItem>();
            foreach(var info in InfoSource)
            {
                var item = new InfoItem(info);
                DisplaySource.Add(item);
            }    
        }
        public void EditInfoItem(System.Windows.Controls.UserControl p)
        {
            if(p.DataContext == null)
                return;
            var item = p.DataContext as InfoItem;
            this._editInfoItemViewModel = new EditInfoItemViewModel(item);
            this.DialogItemViewModel = this._editInfoItemViewModel;
            IsOpen = true;
        }
        public void AddNewInfoItem()
        {
            this._userInfoItemViewModel = new UserInfoItemViewModel();
            this.DialogItemViewModel = this._userInfoItemViewModel;

            IsOpen = true;
        }
        public void ClickImage()
        {
            IsChangeAvatar = !IsChangeAvatar;

        }
        public void ClickChangeImage()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == DialogResult.OK)
            {
                Avatar = op.FileName;
                IsChangeAvatar = false;
            }
            IsChangeAvatar = false;
        }

    }
}
