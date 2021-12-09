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
                if (_selectedRole != "Sinh viên")
                    IsReadOnlyTraining = true;
                if (_selectedRole != "Giáo viên")
                    IsReadOnlyFaculty = true;
            }
        }
        private string _selectedFaculty;
        public string SelectedFaculty { get => _selectedFaculty; set => _selectedFaculty = value; }

        private string _selectedTraining;
        public string SelectedTraining { get => _selectedTraining; set => _selectedTraining = value; }

        public AddStudentListViewModel()
        {
            CurrentStudent = new UserCard();
            NewUser = new User();

            NewUser.Id = Guid.NewGuid();

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
            if (NewUser.DisplayName == null || SelectedTraining == null || SelectedFaculty == null || SelectedRole == null)
                return -1;
            return 0;
        }

        void ConfirmEditStudentInfoFunction()
        {
            if (checkExitCode() != 0)
            {
                MyMessageBox.Show("Mời nhập lại thông tin hợp lệ");
                return;
            }

            UserServices.Instance.SaveUserToDatabase(NewUser);

            CurrentStudent = new UserCard { DisplayName = NewUser.DisplayName, Role = SelectedRole, Faculty = SelectedFaculty, Training = SelectedTraining };

            if (SelectedRole == "Giáo viên")
            {
                
                TeacherServices.Instance.SaveTeacherToDatabase(NewUser);
            }
            
            if (SelectedRole == "Admin")
            {
                Admin newAdmin = new Admin()
                {
                    IdUsers = NewUser.Id,
                   
                };
                AdminServices.Instance.SaveAdminToDatabase(newAdmin);
            }

            if (SelectedRole == "Sinh viên")
            {
                Student newStudent = new Student
                {
                    IdUsers = NewUser.Id,
                    TrainingForm = TrainingFormServices.Instance.FindTrainingFormByDisplayName(SelectedTraining), 
                    
                };
                StudentServices.Instance.SaveStudentToDatabase(newStudent);
            }
               

            ReturnToShowStudentInfo();

        }

        public void ReturnToShowStudentInfo()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            CampusStudentListViewModel campusStudentListViewModel = CampusStudentListViewModel.Instance;
            campusStudentListViewModel.UserDatabase.Add(CurrentStudent);
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel(CurrentStudent);
            campusStudentListViewModel.SearchNameFunction();
        }
    }
}
