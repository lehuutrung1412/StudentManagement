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
    public class StudentListRightSideBarItemEditViewModel : BaseViewModel
    {
        private DetailScore _currentScore;
        public DetailScore CurrentScore { get => _currentScore; set => _currentScore = value; }

        private DetailScore _actualScore;
        public DetailScore ActualScore { get => _actualScore; set => _actualScore = value; }

        public StudentListRightSideBarItemEditViewModel()
        {
            CurrentScore = null;
            ActualScore = null;
        }

        public StudentListRightSideBarItemEditViewModel(DetailScore x)
        {
            CurrentScore = new DetailScore() { QuaTrinh = x.QuaTrinh, CuoiKi = x.CuoiKi, DiemTB = x.DiemTB, GiuaKi = x.GiuaKi, IDStudent = x.IDStudent, IDSubject = x.IDSubject, ThucHanh = x.ThucHanh };
            ActualScore = x;
        }

        public ICommand ConfirmEditDetailScore { get => _confirmEditDetailScore; set => _confirmEditDetailScore = value; }

        private ICommand _confirmEditDetailScore;

        public ICommand CancelEditDetailScore { get => _cancelEditDetailScore; set => _cancelEditDetailScore = value; }

        private ICommand _cancelEditDetailScore;

        public void InitCommand()
        {
            CancelEditDetailScore = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditDetailScoreFunction());
            ConfirmEditDetailScore = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditDetailScoreFunction());
        }

        public void CancelEditDetailScoreFunction()
        {
            CurrentScore = new DetailScore() { QuaTrinh = ActualScore.QuaTrinh, CuoiKi = ActualScore.CuoiKi, DiemTB = ActualScore.DiemTB, GiuaKi = ActualScore.GiuaKi, IDStudent = ActualScore.IDStudent, IDSubject = ActualScore.IDSubject, ThucHanh = ActualScore.ThucHanh };
            ReturnToShowDetailScore();
        }

        public void ConfirmEditDetailScoreFunction()
        {
            ActualScore = CurrentScore;
            ReturnToShowDetailScore();
        }

        public void ReturnToShowDetailScore()
        {
            StudentListRightSideBarViewModel studentListRightSideBarViewModel = StudentListRightSideBarViewModel.Instance;
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new StudentListRightSideBarItemViewModel(CurrentScore);
        }


    }
}
