using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card
        
        public SubjectCard CurrentCard
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                OnPropertyChanged();
            }
        }

        private SubjectCard _currentCard;

        //store card info before edit
        private SubjectCard _actualCard;

        public AdminSubjectClassRightSideBarItemEditViewModel()
        {
            CurrentCard = null;
        }

        public AdminSubjectClassRightSideBarItemEditViewModel(SubjectCard card)
        {
            CurrentCard = new SubjectCard();
            _actualCard = card;
            CurrentCard.CopyCardInfo(card);
            InitCommand();
        }

        public ICommand ConfirmEditSubjectCardInfo { get => _confirmEditSubjectCardInfo; set => _confirmEditSubjectCardInfo = value; }

        private ICommand _confirmEditSubjectCardInfo;

        public ICommand CancelEditSubjectCardInfo { get => _cancelEditSubjectCardInfo; set => _cancelEditSubjectCardInfo = value; }

        private ICommand _cancelEditSubjectCardInfo;

        public void InitCommand()
        {
            CancelEditSubjectCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditSubjectCardInfoFunction());
            ConfirmEditSubjectCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditSubjectCardInfoFunction());
        }

        public void CancelEditSubjectCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(_actualCard);
            ReturnToShowSubjectCardInfo();
        }

        public void ConfirmEditSubjectCardInfoFunction()
        {
            _actualCard.CopyCardInfo(CurrentCard);
            _actualCard.RunOnPropertyChanged();
            ReturnToShowSubjectCardInfo();
        }

        public void ReturnToShowSubjectCardInfo()
        {
            AdminFalcutyTrainingFormRightSideBarViewModel adminFalcutyTrainingFormRightSideBarViewModel = AdminFalcutyTrainingFormRightSideBarViewModel.Instance;
            adminFalcutyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemViewModel(_actualCard);
        }
    }
    
}
