using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminFalcutyTrainingFormViewModel;

namespace StudentManagement.ViewModels
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

        public AdminFalcutyRightSideBarItemEditViewModel()
        {
            this.CurrentCard = null;
        }

        public AdminFalcutyRightSideBarItemEditViewModel(FalcutyCard card)
        {
            this.CurrentCard = new FalcutyCard();
            this._actualCard = card;
            this.CurrentCard.CopyCardInfo(card);
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
            this.CurrentCard.CopyCardInfo(this._actualCard);
            ReturnToShowFalcutyCardInfo();
        }

        public void ConfirmEditFalcutyCardInfoFunction()
        {
            this._actualCard.CopyCardInfo(this.CurrentCard);
            this._actualCard.RunOnPropertyChanged();
            ReturnToShowFalcutyCardInfo();
        }

        public void ReturnToShowFalcutyCardInfo()
        {
            AdminFalcutyTrainingFormRightSideBarViewModel adminFalcutyTrainingFormRightSideBarViewModel = AdminFalcutyTrainingFormRightSideBarViewModel.Instance;
            adminFalcutyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel(this._actualCard);
        }
    }
}
