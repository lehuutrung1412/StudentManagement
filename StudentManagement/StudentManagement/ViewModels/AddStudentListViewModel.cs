using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminStudentListViewModel;

namespace StudentManagement.ViewModels
{
    public class AddStudentListViewModel : BaseViewModel
    {
        private Student _currentStudent;
        public Student CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        public AddStudentListViewModel()
        {
            CurrentStudent = null;

            InitCommand();
        }

        public AddStudentListViewModel(Student x)
        {
            CurrentStudent = new Student();

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
            if (CurrentStudent.Username == null || CurrentStudent.Faculty == null || CurrentStudent.DisplayName == null || CurrentStudent.TrainingForm == null)
                return -1;
            return 0;
        }

        void SetValue(Student a, Student b)
        {
            a.Username = b.Username;
            a.DisplayName = b.DisplayName;
            a.TrainingForm = b.TrainingForm;
            a.Faculty = b.Faculty;
        }

        void ConfirmEditStudentInfoFunction()
        {
            if (checkExitCode() != 0)
            {
                MyMessageBox.Show("Mời nhập lại thông tin hợp lệ");
                return;
            }

            ReturnToShowStudentInfo();

        }

        public void ReturnToShowStudentInfo()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            CampusStudentListViewModel campusStudentListViewModel = CampusStudentListViewModel.Instance;
            campusStudentListViewModel.StudentDatabase.Add(CurrentStudent);
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel(CurrentStudent);
            campusStudentListViewModel.SearchNameFunction();
        }
    }
}
