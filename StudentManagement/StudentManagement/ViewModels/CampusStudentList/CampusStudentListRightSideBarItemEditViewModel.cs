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
    public class CampusStudentListRightSideBarItemEditViewModel : BaseViewModel
    {
        private UserCard _currentStudent;
        public UserCard CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        private User _thisUser;
        public User ThisUser { get => _thisUser; set => _thisUser = value; }

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
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InfoItem> _infoSource;
        public ObservableCollection<InfoItem> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        private string _selectedFaculty;
        public string SelectedFaculty { get => _selectedFaculty; set => _selectedFaculty = value; }

        private string _selectedTraining;
        public string SelectedTraining { get => _selectedTraining; set => _selectedTraining = value; }

        private UserRole _previousRole;
        public UserRole PreviousRole { get => _previousRole; set => _previousRole = value; }

        private string _previousEmail;
        public string PreviousEmail
        {
            get => _previousEmail;
            set => _previousEmail = value;
        }
        public CampusStudentListRightSideBarItemEditViewModel()
        {
            CurrentStudent = null;
            
        }

        public CampusStudentListRightSideBarItemEditViewModel(UserCard x)
        {
            CurrentStudent = x;
            ThisUser = DataProvider.Instance.Database.Users.Where(item => item.Id == x.ID).FirstOrDefault();
            PreviousRole = ThisUser.UserRole;
            SelectedRole = x.Role;
            PreviousEmail = ThisUser.Email;

            

            Roles = new ObservableCollection<string>();
            Roles.Add("Admin");
            Roles.Add("Giáo viên");
            Roles.Add("Sinh viên");


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
            ThisUser.UserRole = PreviousRole;
            ThisUser.IdUserRole = ThisUser.UserRole.Id;
            LoadInfoSource(ThisUser.Id);
            ReturnToShowStudentInfo();
        }

        void PrepairToLoadInfoSource()
        {
            ThisUser.UserRole = DataProvider.Instance.Database.UserRoles.Where(x => x.Role == SelectedRole).FirstOrDefault();
            ThisUser.IdUserRole = ThisUser.UserRole.Id;
            LoadInfoSource(ThisUser.Id);
        }

        public void LoadInfoSource(Guid IdUser)
        {
            var user = UserServices.Instance.GetUserById(IdUser);
            Username = user.Username;

            InfoSource = new ObservableCollection<InfoItem>();

            InfoSource = new ObservableCollection<InfoItem>();
            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Họ và tên", 0, null, user.DisplayName, true));
            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Địa chỉ email", 0, null, user.Email, true));

            user.UserRole = DataProvider.Instance.Database.UserRoles.Where(x => user.IdUserRole == x.Id).FirstOrDefault();

            switch (user.UserRole.Role)
            {
                case "Sinh viên":
                    {
                        var student = StudentServices.Instance.GetStudentbyUser(user);
                        if (student == null)
                        {
                            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), null, true));
                            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), null, true));
                            break;
                        }
                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), student.Faculty.DisplayName, true));
                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), student.TrainingForm.DisplayName, true));
                        break;
                    }
                case "Giáo viên":
                    {
                       
                        var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                        if (lecture == null)
                        {
                            InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), null, true));
                            break;
                        }
                        InfoSource.Add(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), lecture.Faculty.DisplayName, true));
                        break;
                    }
                case "Admin":
                    {
                        foreach (var infoItem in InfoSource)
                            infoItem.IsEnable = true;
                        break;
                    }
            }

            Guid thisId = IdUser;
           

            if (SelectedRole != PreviousRole.Role)
            {
                if (SelectedRole == "Sinh viên")
                    thisId = (Guid)DataProvider.Instance.Database.Students.FirstOrDefault().IdUsers; 

                if (SelectedRole == "Giáo viên")
                    thisId = (Guid)DataProvider.Instance.Database.Teachers.FirstOrDefault().IdUsers;

                if (SelectedRole == "Admin")
                    thisId = (Guid)DataProvider.Instance.Database.Admins.FirstOrDefault().IdUsers;
            }

            ObservableCollection<InfoItem> listInfoItem = InfoItemServices.Instance.GetInfoSourceByUserId(thisId);
            foreach (var infoItem in listInfoItem)
            {
                InfoItem temp = new InfoItem();
                temp.LabelName = infoItem.LabelName;
                temp.Value = infoItem.Value;
                InfoSource.Add(temp);
            }
        }

        bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        int checkExitCode()
        {
            if (InfoSource.First().Value == null || InfoSource[1] == null)
            {
                MyMessageBox.Show("Xin mời nhập thông tin hợp lệ");
                return -1;
            }

            string Email = Convert.ToString(InfoSource[1].Value);
            if (!IsValidEmail(Email))
            {
                MyMessageBox.Show("Email không đúng định dạng");
                return -1;
            }

            if (Email != PreviousEmail && DataProvider.Instance.Database.Users.Where(x => x.Email == Email).FirstOrDefault() != null)
            {
                MyMessageBox.Show("Email đã tồn tại");
                return -1;
            }

            return 0;
        }

        void ConfirmEditStudentInfoFunction()
        {
            if (checkExitCode() != 0)
            { 
                return;
            }

            ThisUser.DisplayName = Convert.ToString(InfoSource.First().Value);
            ThisUser.Email = Convert.ToString(InfoSource[1].Value);

            if (PreviousRole.Role != SelectedRole)
            {
                if (PreviousRole.Role == "Sinh viên")
                {
                    var student = DataProvider.Instance.Database.Students.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();
                    DataProvider.Instance.Database.Students.Remove(student);
                }
                else if (PreviousRole.Role == "Admin")
                {
                    var admin = DataProvider.Instance.Database.Admins.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();
                    DataProvider.Instance.Database.Admins.Remove(admin);
                }
                else
                {
                    var teacher = DataProvider.Instance.Database.Teachers.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();
                    DataProvider.Instance.Database.Teachers.Remove(teacher);
                }

                if (SelectedRole == "Sinh viên")
                {
                    Student newStudent = new Student();
                    newStudent.IdUsers = ThisUser.Id;
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
                    newTeacher.IdUsers = ThisUser.Id;
                    newTeacher.Id = Guid.NewGuid();
                    string temp = Convert.ToString(InfoSource[2].Value);
                    newTeacher.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                    newTeacher.IdFaculty = newTeacher.Faculty.Id;

                    TeacherServices.Instance.SaveTeacherToDatabase(newTeacher);
                }
                else
                {
                    Admin newAdmin = new Admin();
                    newAdmin.IdUsers = ThisUser.Id;
                    newAdmin.Id = Guid.NewGuid();

                    AdminServices.Instance.SaveAdminToDatabase(newAdmin);
                }
            }
            else
            {
                if (SelectedRole == "Sinh viên")
                {
                    Student currentStudent = DataProvider.Instance.Database.Students.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();

                    string temp = Convert.ToString(InfoSource[2].Value);
                    currentStudent.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                    temp = Convert.ToString(InfoSource[3].Value);
                    currentStudent.TrainingForm = DataProvider.Instance.Database.TrainingForms.Where(x => x.DisplayName == temp).FirstOrDefault();
                    currentStudent.IdFaculty = currentStudent.Faculty.Id;
                    currentStudent.IdTrainingForm = currentStudent.TrainingForm.Id;
                }
                else if (SelectedRole == "Giáo viên")
                {
                    Teacher currentTeacher = DataProvider.Instance.Database.Teachers.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();
                    string temp = Convert.ToString(InfoSource[2].Value);
                    currentTeacher.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                    currentTeacher.IdFaculty = currentTeacher.Faculty.Id;
                }
            }

            if (SelectedRole != PreviousRole.Role)
            {
                var deleteRoleInfo = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(x => x.IdUser == ThisUser.Id);
                foreach (var item in deleteRoleInfo)
                {
                    item.IdUser = null;
                }
            }

            foreach (var item in InfoSource)
            {
                if (item.LabelName != "Hệ đào tạo" && item.LabelName != "Khoa" && item.LabelName != "Họ và tên" && item.LabelName != "Địa chỉ email")
                { 

                    User_UserRole_UserInfo newInfo = new User_UserRole_UserInfo();
                    newInfo.Id = Guid.NewGuid();
                    newInfo.IdUser = ThisUser.Id;
                    newInfo.IdUserRole_Info = DataProvider.Instance.Database.UserRole_UserInfo.Where(x => x.InfoName == item.LabelName).FirstOrDefault().Id;
                    newInfo.Content = Convert.ToString(item.Value);
                    UserUserRoleUserInfoServices.Instance.SaveUserInfoToDatabase(newInfo);
                }
            }


            DataProvider.Instance.Database.SaveChanges();

            ReturnToShowStudentInfo();
        }

        public void ReturnToShowStudentInfo()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            CampusStudentListViewModel campusStudentListViewModel = CampusStudentListViewModel.Instance;
            var userCard = campusStudentListViewModel.UserDatabase.Where(x => x.ID == ThisUser.Id).FirstOrDefault();
            userCard.Role = ThisUser.UserRole.Role;
            campusStudentListViewModel.SearchNameFunction();
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel(InfoSource);
        }
    }
}
