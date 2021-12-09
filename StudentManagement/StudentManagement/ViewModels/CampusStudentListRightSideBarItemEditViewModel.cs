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

namespace StudentManagement.ViewModels
{
    public class CampusStudentListRightSideBarItemEditViewModel : BaseViewModel
    {
        private UserCard _currentStudent;
        public UserCard CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        private UserCard _actualStudent;
        public UserCard ActualStudent { get => _actualStudent; set => _actualStudent = value; }

        private ObservableCollection<string> _trainings;
        public ObservableCollection<string> Trainings { get => _trainings; set => _trainings = value; }

        private ObservableCollection<string> _faculties;
        public ObservableCollection<string> Faculties
        {
            get => _faculties;
            set => _faculties = value;
        }

        private string _selectedFaculty;
        public string SelectedFaculty { get => _selectedFaculty; set => _selectedFaculty = value; }

        private string _selectedTraining;
        public string SelectedTraining { get => _selectedTraining; set => _selectedTraining = value; }

        public CampusStudentListRightSideBarItemEditViewModel()
        {
            CurrentStudent = null;
            ActualStudent = null;
        }

        public CampusStudentListRightSideBarItemEditViewModel(UserCard x)
        {
            CurrentStudent = new UserCard() { ID = x.ID, DisplayName = x.DisplayName, Faculty = x.Faculty, Email = x.Email, Gender = x.Gender, Training = x.Training};
            ActualStudent = x;

            var trainingForms = TrainingFormServices.Instance.LoadTrainingFormList();
            var faculties = FacultyServices.Instance.LoadFacultyList();

            Faculties = new ObservableCollection<string>();
            Trainings = new ObservableCollection<string>();

            foreach (var item in trainingForms)
                Trainings.Add(item.DisplayName);

            foreach (var item in faculties)
                Faculties.Add(item.DisplayName);

            SelectedFaculty = null;
            SelectedTraining = null;

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
            ReturnToShowStudentInfo();
        }

        int checkExitCode()
        {
            if (CurrentStudent.DisplayName == null || CurrentStudent.Faculty == null || CurrentStudent.Training == null)
                return -1;
            return 0;
        }

        void SetValue(UserCard a, UserCard b)
        {
            a.DisplayName = b.DisplayName;
           
            a.Training = SelectedTraining;
            a.Faculty = SelectedFaculty;
        }

        void ConfirmEditStudentInfoFunction()
        {
            if (checkExitCode() != 0)
            {
                MyMessageBox.Show("Mời nhập lại thông tin hợp lệ");
                return;
            }

            SetValue(ActualStudent, CurrentStudent);
            ReturnToShowStudentInfo();
              
        }

        public void ReturnToShowStudentInfo()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel(ActualStudent);
        }
    }
}
