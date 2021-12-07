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
    public class AdminSubjectRightSideBarItemEditViewModel : BaseViewModel
    {
        #region properties
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
        public SubjectCard ActualCard { get => _actualCard; set => _actualCard = value; }

        #endregion

        #region icommands
        private ICommand _confirmEditSubjectCardInfo;
        private ICommand _cancelEditSubjectCardInfo;
        public ICommand ConfirmEditSubjectCardInfo { get => _confirmEditSubjectCardInfo; set => _confirmEditSubjectCardInfo = value; }
        public ICommand CancelEditSubjectCardInfo { get => _cancelEditSubjectCardInfo; set => _cancelEditSubjectCardInfo = value; }
        #endregion
        public AdminSubjectRightSideBarItemEditViewModel()
        {
            CurrentCard = null;
        }

        public AdminSubjectRightSideBarItemEditViewModel(SubjectCard card, bool isCreatedNew = false)
        {
            CurrentCard = new SubjectCard();
            ActualCard = card;
            if (!isCreatedNew)
            {
                CurrentCard.CopyCardInfo(card);
            }
            InitCommand();
        }

        #region methods

        public void InitCommand()
        {
            CancelEditSubjectCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditSubjectCardInfoFunction());
            ConfirmEditSubjectCardInfo = new RelayCommand<object>((p) =>
            {
                if (!CurrentCard.HasErrors && CanConfirmEdit())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (p) => ConfirmEditSubjectCardInfoFunction());
        }

        public bool CanConfirmEdit()
        {
            if (!string.IsNullOrEmpty(CurrentCard.DisplayName) && !string.IsNullOrEmpty(CurrentCard.Code) && !string.IsNullOrEmpty(CurrentCard.Credit.ToString()))

                return true;
            return false;
        }

        public void CancelEditSubjectCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowSubjectCardInfo();
        }

        public void ConfirmEditSubjectCardInfoFunction()
        {
            bool isCardExist = AdminSubjectViewModel.SubjectCards.Contains(ActualCard);
            ActualCard.CopyCardInfo(CurrentCard);

            // check if card exist -> Not exist insert new
            if (!isCardExist)
            {
                AdminSubjectViewModel.SubjectCards.Insert(1, ActualCard);
                AdminSubjectViewModel.StoredSubjectCards.Insert(1, ActualCard);
            }

            SubjectServices.Instance.SaveSubjectCardToDatabase(ActualCard);

            ActualCard.RunOnPropertyChanged();
            ReturnToShowSubjectCardInfo();
        }


        public void ReturnToShowSubjectCardInfo()
        {
            AdminSubjectRightSideBarViewModel adminSubjectRightSideBarViewModel = AdminSubjectRightSideBarViewModel.Instance;
            adminSubjectRightSideBarViewModel.RightSideBarItemViewModel = new AdminSubjectRightSideBarItemViewModel(ActualCard);
        }
        #endregion

    }
}
