using StudentManagement.Commands;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminFacultyTrainingFormViewModel;

namespace StudentManagement.ViewModels
{
    class AdminFacultyRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card
        public FacultyCard CurrentCard
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                OnPropertyChanged();
            }
        }

        private FacultyCard _currentCard;

        //store card info before edit
        private FacultyCard _actualCard;
        public FacultyCard ActualCard { get => _actualCard; set => _actualCard = value; }

        public AdminFacultyRightSideBarItemEditViewModel()
        {
            CurrentCard = null;
        }

        public AdminFacultyRightSideBarItemEditViewModel(FacultyCard card)
        {
            CurrentCard = new FacultyCard();
            ActualCard = card;
            CurrentCard.CopyCardInfo(card);
            InitCommand();
        }

        public ICommand ConfirmEditFacultyCardInfo { get => _confirmEditFacultyCardInfo; set => _confirmEditFacultyCardInfo = value; }

        private ICommand _confirmEditFacultyCardInfo;

        public ICommand CancelEditFacultyCardInfo { get => _cancelEditFacultyCardInfo; set => _cancelEditFacultyCardInfo = value; }

        private ICommand _cancelEditFacultyCardInfo;

        public void InitCommand()
        {
            CancelEditFacultyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditFacultyCardInfoFunction());
            ConfirmEditFacultyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditFacultyCardInfoFunction());
        }

        public void CancelEditFacultyCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowFacultyCardInfo();
        }

        public void ConfirmEditFacultyCardInfoFunction()
        {
            ActualCard.CopyCardInfo(CurrentCard);
            ActualCard.RunOnPropertyChanged();
            ReturnToShowFacultyCardInfo();
        }

        public void ReturnToShowFacultyCardInfo()
        {
            AdminFacultyTrainingFormRightSideBarViewModel adminFacultyTrainingFormRightSideBarViewModel = AdminFacultyTrainingFormRightSideBarViewModel.Instance;
            adminFacultyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminFacultyRightSideBarItemViewModel(ActualCard);
        }
    }
}
