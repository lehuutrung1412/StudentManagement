using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AddStudentListViewModel : BaseViewModel
    {
        private UserCard _currentStudent;
        public UserCard CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        private User _newUser;
        public User NewUser { get => _newUser; set => _newUser = value; }

        private ObservableCollection<string> _trainings;
        public ObservableCollection<string> Trainings { get => _trainings; set => _trainings = value; }

        private ObservableCollection<string> _faculties;
        public ObservableCollection<string> Faculties
        {
            get => _faculties;
            set => _faculties = value;
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => _username = value;
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        private string _rePassword;
        public string RePassword
        {
            get => _rePassword;
            set => _rePassword = value;
        }

        private ObservableCollection<string> _roles;
        public ObservableCollection<string> Roles { get => _roles; set => _roles = value; }

        private bool _isReadOnlyTraining;
        public bool IsReadOnlyTraining
        {
            get => _isReadOnlyTraining;
            set
            {
                _isReadOnlyTraining = value;
                OnPropertyChanged();
            }
        }
        
        private bool _isReadOnlyFaculty;
        public bool IsReadOnlyFaculty
        {
            get => _isReadOnlyFaculty;
            set
            {
                _isReadOnlyFaculty = value;
                OnPropertyChanged();
            }
        }

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (_selectedRole != null)
                    PrepairToLoadInfoSource();
            }
        }

        private ObservableCollection<InfoItem> _infoSource;
        public ObservableCollection<InfoItem> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private string _selectedFaculty;
        public string SelectedFaculty { get => _selectedFaculty; set => _selectedFaculty = value; }

        private string _selectedTraining;
        public string SelectedTraining { get => _selectedTraining; set => _selectedTraining = value; }

        public AddStudentListViewModel()
        {
            CurrentStudent = new UserCard();
           

            var trainingForms = TrainingFormServices.Instance.LoadTrainingFormList();
            var faculties = FacultyServices.Instance.LoadFacultyList();

            Faculties = new ObservableCollection<string>();
            Trainings = new ObservableCollection<string>();
            Roles = new ObservableCollection<string>();

            Roles.Add("Admin");
            Roles.Add("Giáo viên");
            Roles.Add("Sinh viên");

            foreach (var item in trainingForms)
                Trainings.Add(item.DisplayName);

            foreach (var item in faculties)
                Faculties.Add(item.DisplayName);

            IsReadOnlyFaculty = false;
            IsReadOnlyTraining = false;
            

            SelectedFaculty = null;
            SelectedTraining = null;
            SelectedRole = null;

            InitCommand();
        }


        public ICommand ConfirmEditStudentInfo { get => _confirmEditStudentInfo; set => _confirmEditStudentInfo = value; }

        private ICommand _confirmEditStudentInfo;

        public ICommand CancelEditStudentInfo { get => _cancelEditStudentInfo; set => _cancelEditStudentInfo = value; }

        private ICommand _cancelEditStudentInfo;

        public void InitCommand()
        {
            CancelEditStudentInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditStudentInfoFunction());
            ConfirmEditStudentInfo = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditStudentInfoFunction());
        }

        void CancelEditStudentInfoFunction()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new EmptyStateRightSideBarViewModel();
        }

        int checkExitCode()
        {
            if (InfoSource.First().Value == null || Username == null || Password == null || RePassword == null || InfoSource[1] == null)
            {
                MyMessageBox.Show("Xin mời nhập thông tin hợp lệ");
                return -1;
            }

            if (DataProvider.Instance.Database.Users.Where(x => x.Username == Username).FirstOrDefault() != null)
            {
                MyMessageBox.Show("Đã tồn tại username, mời nhập lại username");
                return -1;
            }

            if (Password != RePassword)
            {
                MyMessageBox.Show("Hai mật khẩu không khớp nhau");
                return -1;
            }

            return 0;
        }

        void PrepairToLoadInfoSource()
        {
            if (SelectedRole == "Sinh viên")
                LoadInfoSource((Guid)DataProvider.Instance.Database.Students.FirstOrDefault().IdUsers);
            else if (SelectedRole == "Giáo viên")
                LoadInfoSource((Guid)DataProvider.Instance.Database.Teachers.FirstOrDefault().IdUsers);
            else if (SelectedRole == "Admin")
                LoadInfoSource((Guid)DataProvider.Instance.Database.Admins.FirstOrDefault().IdUsers);
        }

        public void LoadInfoSource(Guid IdUser)
        {
            var user = UserServices.Instance.GetUserById(IdUser);
            
            InfoSource = new ObservableCollection<InfoItem>();

            InfoSource = new ObservableCollection<InfoItem>();
            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Họ và tên", 0, null, null, true));
            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Địa chỉ email", 0, null, null, true));

            user.UserRole = DataProvider.Instance.Database.UserRoles.Where(x => user.IdUserRole == x.Id).FirstOrDefault();

            switch (user.UserRole.Role)
            {
                case "Sinh viên":
                    {
                        var student = StudentServices.Instance.GetStudentbyUser(user);

                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), null, true));
                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), null, true));
                        break;
                    }
                case "Giáo viên":
                    {
                        var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), null, true));
                        break;
                    }
                case "Admin":
                    {
                        foreach (var infoItem in InfoSource)
                            infoItem.IsEnable = true;
                        break;
                    }
            }

            var listInfoItem = InfoItemServices.Instance.GetInfoSourceByUserId(IdUser);
            foreach (var infoItem in listInfoItem)
            {
                InfoSource.Add(infoItem);
            }
        }

        void ConfirmEditStudentInfoFunction()
        {
            if (checkExitCode() != 0)
            {
                return;
            }

            NewUser = new User();
            
            NewUser.Id = Guid.NewGuid();
            NewUser.Username = Username;
            NewUser.Password = Password;
            NewUser.DisplayName = Convert.ToString(InfoSource.First().Value);
            NewUser.Email = Convert.ToString(InfoSource[1].Value);
            NewUser.UserRole = DataProvider.Instance.Database.UserRoles.Where(x => x.Role == SelectedRole).FirstOrDefault();
            NewUser.IdUserRole = NewUser.UserRole.Id;

            UserServices.Instance.SaveUserToDatabase(NewUser);
            
            if (SelectedRole == "Sinh viên")
            {
                Student newStudent = new Student();
                newStudent.IdUsers = NewUser.Id;
                newStudent.Id = Guid.NewGuid();
                string temp = Convert.ToString(InfoSource[2].Value);
                newStudent.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                temp = Convert.ToString(InfoSource[3].Value);
                newStudent.TrainingForm = DataProvider.Instance.Database.TrainingForms.Where(x => x.DisplayName == temp).FirstOrDefault();
                newStudent.IdFaculty = newStudent.Faculty.Id;
                newStudent.IdTrainingForm = newStudent.TrainingForm.Id;

                StudentServices.Instance.SaveStudentToDatabase(newStudent);
            }
            else if (SelectedRole == "Giáo viên")
            {
                Teacher newTeacher = new Teacher();
                newTeacher.IdUsers = NewUser.Id;
                newTeacher.Id = Guid.NewGuid();
                string temp = Convert.ToString(InfoSource[2].Value);
                newTeacher.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                newTeacher.IdFaculty = newTeacher.Faculty.Id;

                TeacherServices.Instance.SaveTeacherToDatabase(newTeacher);
            }
            else
            {
                Admin newAdmin = new Admin();
                newAdmin.IdUsers = NewUser.Id;
                newAdmin.Id = Guid.NewGuid();

                AdminServices.Instance.SaveAdminToDatabase(newAdmin);

            }

            foreach (var item in InfoSource)
            {
                if (item.LabelName != "Hệ đào tạo" && item.LabelName != "Khoa")
                {
                    User_UserRole_UserInfo newInfo = new User_UserRole_UserInfo();
                    newInfo.Id = Guid.NewGuid();
                    newInfo.IdUser = NewUser.Id;
                    newInfo.Content = Convert.ToString(item.Value);
                    UserUserRoleUserInfoServices.Instance.SaveUserInfoToDatabase(newInfo);
                }
            }

            CurrentStudent.DisplayName = NewUser.DisplayName;
            CurrentStudent.Email = NewUser.Email;
            CurrentStudent.ID = NewUser.Id;
            CurrentStudent.Role = SelectedRole;

            ReturnToShowStudentInfo();

        }

        public void ReturnToShowStudentInfo()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            CampusStudentListViewModel campusStudentListViewModel = CampusStudentListViewModel.Instance;
            campusStudentListViewModel.UserDatabase.Add(CurrentStudent);
            studentListRightSideBarViewModel.ShowStudentCardInfoDetail(CurrentStudent);
            campusStudentListViewModel.SearchNameFunction();
        }
    }
}
