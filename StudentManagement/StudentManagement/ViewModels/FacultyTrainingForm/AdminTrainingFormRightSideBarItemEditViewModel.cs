using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminFacultyTrainingFormViewModel;

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

        public AdminTrainingFormRightSideBarItemEditViewModel(TrainingFormCard card, bool isCreatedNew = false)
        {
            CurrentCard = new TrainingFormCard();
            ActualCard = card;
            if (!isCreatedNew)
            {
                CurrentCard.CopyCardInfo(card);
            }
            InitCommand();
        }

        public ICommand ConfirmEditTrainingFormCardInfo { get => _confirmEditTrainingFormCardInfo; set => _confirmEditTrainingFormCardInfo = value; }

        private ICommand _confirmEditTrainingFormCardInfo;

        public ICommand CancelEditTrainingFormCardInfo { get => _cancelEditTrainingFormCardInfo; set => _cancelEditTrainingFormCardInfo = value; }
        public TrainingFormCard ActualCard { get => _actualCard; set => _actualCard = value; }

        private ICommand _cancelEditTrainingFormCardInfo;

        public void InitCommand()
        {
            CancelEditTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditTrainingFormCardInfoFunction());
            ConfirmEditTrainingFormCardInfo = new RelayCommand<object>((p) =>
            {
                if (!CurrentCard.HasErrors && CanConfirmEdit())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (p) => ConfirmEditTrainingFormCardInfoFunction());
        }

        public bool CanConfirmEdit()
        {
            if (!string.IsNullOrEmpty(CurrentCard.DisplayName))

                return true;
            return false;
        }

        public void CancelEditTrainingFormCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowTrainingFormCardInfo();
        }

        public void ConfirmEditTrainingFormCardInfoFunction()
        {
            bool isCardExist = AdminFacultyTrainingFormViewModel.TrainingFormCards.Contains(ActualCard);

            // store current actual
            TrainingFormCard storedActualCard = new TrainingFormCard();
            storedActualCard.CopyCardInfo(ActualCard);

            // copy current card property to actual card
            ActualCard.CopyCardInfo(CurrentCard);

            bool success = TrainingFormServices.Instance.SaveTrainingFormCardToDatabase(ActualCard);

            if (success)
            {
                // check if card exist -> Not exist insert new
                if (!isCardExist)
                {
                    AdminFacultyTrainingFormViewModel.TrainingFormCards.Insert(1, ActualCard);
                }
                MyMessageBox.Show("Thêm/chỉnh sửa hệ đào tạo thành công");
            }
            else
            {
                // rollback to previous actual card
                ActualCard.CopyCardInfo(storedActualCard);
                MyMessageBox.Show("Có lỗi kết nối đến cơ sở dữ liệu, vui lòng thử lại sau");
            }

            ActualCard.RunOnPropertyChanged();
            ReturnToShowTrainingFormCardInfo();
        }

        public void ReturnToShowTrainingFormCardInfo()
        {
            AdminFacultyTrainingFormRightSideBarViewModel adminFacultyTrainingFormRightSideBarViewModel = AdminFacultyTrainingFormRightSideBarViewModel.Instance;
            adminFacultyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel(ActualCard);
        }
    }
}
