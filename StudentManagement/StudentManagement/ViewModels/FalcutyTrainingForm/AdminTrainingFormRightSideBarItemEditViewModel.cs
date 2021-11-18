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
    public class AdminTrainingFormRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card
        public TrainingFormCard CurrentCard
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                OnPropertyChanged();
            }
        }

        private TrainingFormCard _currentCard;

        //store card info before edit
        private TrainingFormCard _actualCard;

        public AdminTrainingFormRightSideBarItemEditViewModel()
        {
            CurrentCard = null;
        }

        public AdminTrainingFormRightSideBarItemEditViewModel(TrainingFormCard card)
        {
            CurrentCard = new TrainingFormCard();
            _actualCard = card;
            CurrentCard.CopyCardInfo(card);
            InitCommand();
        }

        public ICommand ConfirmEditTrainingFormCardInfo { get => _confirmEditTrainingFormCardInfo; set => _confirmEditTrainingFormCardInfo = value; }

        private ICommand _confirmEditTrainingFormCardInfo;

        public ICommand CancelEditTrainingFormCardInfo { get => _cancelEditTrainingFormCardInfo; set => _cancelEditTrainingFormCardInfo = value; }

        private ICommand _cancelEditTrainingFormCardInfo;

        public void InitCommand()
        {
            CancelEditTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditTrainingFormCardInfoFunction());
            ConfirmEditTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditTrainingFormCardInfoFunction());
        }

        public void CancelEditTrainingFormCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(_actualCard);
            ReturnToShowTrainingFormCardInfo();
        }

        public void ConfirmEditTrainingFormCardInfoFunction()
        {
            _actualCard.CopyCardInfo(CurrentCard);
            _actualCard.RunOnPropertyChanged();
            ReturnToShowTrainingFormCardInfo();
        }

        public void ReturnToShowTrainingFormCardInfo()
        {
            AdminFalcutyTrainingFormRightSideBarViewModel adminFalcutyTrainingFormRightSideBarViewModel = AdminFalcutyTrainingFormRightSideBarViewModel.Instance;
            adminFalcutyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel(_actualCard);
        }
    }
}
