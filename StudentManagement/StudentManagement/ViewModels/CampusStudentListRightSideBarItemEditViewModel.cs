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
    public class CampusStudentListRightSideBarItemEditViewModel : BaseViewModel
    {
        private StudentGrid _currentStudent;
        public StudentGrid CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        private StudentGrid _actualStudent;
        public StudentGrid ActualStudent { get => _actualStudent; set => _actualStudent = value; }

        public CampusStudentListRightSideBarItemEditViewModel()
        {
            CurrentStudent = null;
            ActualStudent = null;
        }

        public CampusStudentListRightSideBarItemEditViewModel(StudentGrid x)
        {
            CurrentStudent = new StudentGrid() { Username = x.Username, Faculty = x.Faculty, Email = x.Email, Gender = x.Gender, DisplayName = x.DisplayName, TrainingForm = x.TrainingForm, Status = x.Status };
            ActualStudent = x;

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

        void SetValue(StudentGrid a, StudentGrid b)
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
