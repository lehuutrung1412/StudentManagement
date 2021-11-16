using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.FalcutyTrainingForm.AdminFalcutyTrainingFormViewModel;

namespace StudentManagement.ViewModels.FalcutyTrainingForm
{
    class AdminFalcutyRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card
        public FalcutyCard CurrentCard
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                OnPropertyChanged();
            }
        }

        private FalcutyCard _currentCard;

        //store card info before edit
        private FalcutyCard _actualCard;
        public FalcutyCard ActualCard { get => _actualCard; set => _actualCard = value; }

        public AdminFalcutyRightSideBarItemEditViewModel()
        {
            CurrentCard = null;
        }

        public AdminFalcutyRightSideBarItemEditViewModel(FalcutyCard card)
        {
            CurrentCard = new FalcutyCard();
            ActualCard = card;
            CurrentCard.CopyCardInfo(card);
            InitCommand();
        }

        public ICommand ConfirmEditFalcutyCardInfo { get => _confirmEditFalcutyCardInfo; set => _confirmEditFalcutyCardInfo = value; }

        private ICommand _confirmEditFalcutyCardInfo;

        public ICommand CancelEditFalcutyCardInfo { get => _cancelEditFalcutyCardInfo; set => _cancelEditFalcutyCardInfo = value; }

        private ICommand _cancelEditFalcutyCardInfo;

        public void InitCommand()
        {
            CancelEditFalcutyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditFalcutyCardInfoFunction());
            ConfirmEditFalcutyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditFalcutyCardInfoFunction());
        }

        public void CancelEditFalcutyCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowFalcutyCardInfo();
        }

        public void ConfirmEditFalcutyCardInfoFunction()
        {
            ActualCard.CopyCardInfo(CurrentCard);
            ActualCard.RunOnPropertyChanged();
            ReturnToShowFalcutyCardInfo();
        }

        public void ReturnToShowFalcutyCardInfo()
        {
            AdminFalcutyTrainingFormRightSideBarViewModel adminFalcutyTrainingFormRightSideBarViewModel = AdminFalcutyTrainingFormRightSideBarViewModel.Instance;
            adminFalcutyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel(ActualCard);
        }
    }
}
