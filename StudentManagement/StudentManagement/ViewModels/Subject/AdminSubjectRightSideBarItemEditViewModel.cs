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

        public bool IsCreateNew { get; set; }

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
            IsCreateNew = isCreatedNew;
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
            if (string.IsNullOrEmpty(CurrentCard.DisplayName) || string.IsNullOrEmpty(CurrentCard.Code) || string.IsNullOrEmpty(CurrentCard.Credit.ToString()))

                return false;
            if (IsCreateNew && AdminSubjectViewModel.StoredSubjectCards.Where(subject => subject.Code == CurrentCard.Code).Count() > 0)
                return false;
            return true;
        }

        public void CancelEditSubjectCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowSubjectCardInfo();
        }

        public void ConfirmEditSubjectCardInfoFunction()
        {
            bool isCardExist = AdminSubjectViewModel.SubjectCards.Contains(ActualCard);

            // store current actual
            SubjectCard storedActualCard = new SubjectCard();
            storedActualCard.CopyCardInfo(ActualCard);

            // copy current card property to actual card
            ActualCard.CopyCardInfo(CurrentCard);

            bool success = SubjectServices.Instance.SaveSubjectCardToDatabase(ActualCard);

            if (success)
            {
                // check if card exist -> Not exist insert new
                if (!isCardExist)
                {
                    AdminSubjectViewModel.SubjectCards.Insert(0, ActualCard);
                    AdminSubjectViewModel.StoredSubjectCards.Insert(0, ActualCard);
                }
                MyMessageBox.Show("Thêm/chỉnh sửa  môn học thành công");
            }
            else
            {
                // rollback to previous actual card
                ActualCard.CopyCardInfo(storedActualCard);
                MyMessageBox.Show("Có lỗi kết nối đến cơ sở dữ liệu, vui lòng thử lại sau");
            }


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
