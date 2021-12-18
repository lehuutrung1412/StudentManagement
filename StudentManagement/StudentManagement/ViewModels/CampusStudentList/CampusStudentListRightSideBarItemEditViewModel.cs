using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.ViewModels.UserInfo;
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

        private ObservableCollection<InfoItemViewModel> _infoSource;
        public ObservableCollection<InfoItemViewModel> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

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
            ConfirmEditStudentInfo = new RelayCommand<object>((p) => { return CanEdit(); }, (p) => ConfirmEditStudentInfoFunction());
        }

        bool CanEdit()
        {
            if (SelectedRole == null)
                return false;

            int num = 4;
            if (SelectedRole == "Giáo viên")
                num = 3;
            else if (SelectedRole == "Admin")
                num = 2;

            for (int i = 0; i < num; ++i)
            {
                if (InfoSource[i].HasErrors || string.IsNullOrEmpty(InfoSource[i].Content))
                    return false;
            }

            return true;
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
            try
            {
                var user = UserServices.Instance.GetUserById(IdUser);
                Username = user.Username;

                InfoSource = new ObservableCollection<InfoItemViewModel>();

                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Họ và tên", 0, null, user.DisplayName, true)));
                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Địa chỉ email", 0, null, user.Email, true)));

                user.UserRole = DataProvider.Instance.Database.UserRoles.Where(x => user.IdUserRole == x.Id).FirstOrDefault();

                switch (user.UserRole.Role)
                {
                    case "Sinh viên":
                        {
                            var student = StudentServices.Instance.GetStudentbyUser(user);
                            if (student == null)
                            {
                                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), null, true)));
                                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), null, true)));
                                break;
                            }
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), student.Faculty.DisplayName, true)));
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), student.TrainingForm.DisplayName, true)));
                            break;
                        }
                    case "Giáo viên":
                        {

                            var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                            if (lecture == null)
                            {
                                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), null, true)));
                                break;
                            }
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), lecture.Faculty.DisplayName, true)));
                            break;
                        }
                    case "Admin":
                        {
                            foreach (var infoItem in InfoSource)
                                infoItem.CurrendInfoItem.IsEnable = true;
                            break;
                        }
                }

                Guid thisId = IdUser;
                ObservableCollection<InfoItem> listInfoItem = InfoItemServices.Instance.GetInfoSourceByUserId(thisId);
                foreach (var infoItem in listInfoItem)
                {
                    InfoSource.Add(new InfoItemViewModel(infoItem));
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
            }
        }

        int checkExitCode()
        {
            if (InfoSource.First().Content == null || InfoSource[1] == null)
            {
                MyMessageBox.Show("Xin mời nhập thông tin hợp lệ");
                return -1;
            }

            string Email = Convert.ToString(InfoSource[1].Content);

            if (Email != PreviousEmail && DataProvider.Instance.Database.Users.Where(x => x.Email == Email).FirstOrDefault() != null)
            {
                MyMessageBox.Show("Email đã tồn tại");
                return -1;
            }

            return 0;
        }

        void ConfirmEditStudentInfoFunction()
        {
            try
            {


                if (checkExitCode() != 0)
                {
                    return;
                }

                ThisUser.DisplayName = Convert.ToString(InfoSource.First().Content);
                ThisUser.Email = Convert.ToString(InfoSource[1].Content);


                {
                    if (SelectedRole == "Sinh viên")
                    {
                        Student currentStudent = DataProvider.Instance.Database.Students.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();

                        string temp = Convert.ToString(InfoSource[2].Content);
                        currentStudent.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                        temp = Convert.ToString(InfoSource[3].Content);
                        currentStudent.TrainingForm = DataProvider.Instance.Database.TrainingForms.Where(x => x.DisplayName == temp).FirstOrDefault();
                        currentStudent.IdFaculty = currentStudent.Faculty.Id;
                        currentStudent.IdTrainingForm = currentStudent.TrainingForm.Id;
                    }
                    else if (SelectedRole == "Giáo viên")
                    {
                        Teacher currentTeacher = DataProvider.Instance.Database.Teachers.Where(x => x.IdUsers == ThisUser.Id).FirstOrDefault();
                        string temp = Convert.ToString(InfoSource[2].Content);
                        currentTeacher.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                        currentTeacher.IdFaculty = currentTeacher.Faculty.Id;
                    }
                }


                foreach (var item in InfoSource)
                {
                    if (item.CurrendInfoItem.LabelName != "Hệ đào tạo" && item.CurrendInfoItem.LabelName != "Khoa" && item.CurrendInfoItem.LabelName != "Họ và tên" && item.CurrendInfoItem.LabelName != "Địa chỉ email")
                    {
                        var findInfo = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(x => x.IdUser == ThisUser.Id && x.UserRole_UserInfo.InfoName == item.CurrendInfoItem.LabelName).FirstOrDefault();
                        if (findInfo != null)
                        {
                            findInfo.Content = Convert.ToString(item.Content);
                        }
                    }
                }


                DataProvider.Instance.Database.SaveChanges();

                ReturnToShowStudentInfo();
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
            }
        }

        public void ReturnToShowStudentInfo()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            CampusStudentListViewModel campusStudentListViewModel = CampusStudentListViewModel.Instance;
            var userCard = campusStudentListViewModel.UserDatabase.Where(x => x.ID == ThisUser.Id).FirstOrDefault();
            userCard.DisplayName = ThisUser.DisplayName;
            campusStudentListViewModel.SearchNameFunction();
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel(InfoSource);
        }
    }
}
