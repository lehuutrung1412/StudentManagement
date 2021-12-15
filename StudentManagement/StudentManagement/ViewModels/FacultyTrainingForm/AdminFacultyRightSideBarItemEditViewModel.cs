using StudentManagement.Commands;
using StudentManagement.Models;
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

        public AdminFacultyRightSideBarItemEditViewModel(FacultyCard card, bool isCreatedNew = false)
        {
            CurrentCard = new FacultyCard();
            ActualCard = card;
            ActualCard.InitTrainingFormOfFacultyList();
            if (!isCreatedNew)
            {
                CurrentCard.CopyCardInfo(card);
            }
            InitCommand();
        }

        public ICommand ConfirmEditFacultyCardInfo { get => _confirmEditFacultyCardInfo; set => _confirmEditFacultyCardInfo = value; }

        private ICommand _confirmEditFacultyCardInfo;

        public ICommand CancelEditFacultyCardInfo { get => _cancelEditFacultyCardInfo; set => _cancelEditFacultyCardInfo = value; }

        private ICommand _cancelEditFacultyCardInfo;

        public ICommand AddToTrainingFormsListOfFaculty { get => _addToTrainingFormsListOfFaculty; set => _addToTrainingFormsListOfFaculty = value; }

        private ICommand _addToTrainingFormsListOfFaculty;

        public ICommand RemoveFromTrainingFormsListOfFaculty { get => _removeFromTrainingFormsListOfFaculty; set => _removeFromTrainingFormsListOfFaculty = value; }

        private ICommand _removeFromTrainingFormsListOfFaculty;



        public void InitCommand()
        {
            CancelEditFacultyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditFacultyCardInfoFunction());
            ConfirmEditFacultyCardInfo = new RelayCommand<object>((p) =>
            {
                if (!CurrentCard.HasErrors && CanConfirmEdit())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (p) => ConfirmEditFacultyCardInfoFunction());
            AddToTrainingFormsListOfFaculty = new RelayCommand<TrainingForm>((p) =>
            {
                if (p != null)
                {
                    return true;
                }
                return false;
            }, (p) => AddToTrainingFormsListOfFacultyFunction(p));
            RemoveFromTrainingFormsListOfFaculty = new RelayCommand<TrainingForm>((p) => { return true; }, (p) => RemoveFromTrainingFormsListOfFacultyFunction(p));
        }

        public bool CanConfirmEdit()
        {
            if (!string.IsNullOrEmpty(CurrentCard.DisplayName))

                return true;
            return false;
        }

        public void CancelEditFacultyCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ActualCard.InitTrainingFormOfFacultyList();
            ReturnToShowFacultyCardInfo();
        }

        public void ConfirmEditFacultyCardInfoFunction()
        {
            bool isCardExist = AdminFacultyTrainingFormViewModel.StoredFacultyCards.Contains(ActualCard);

            // store current actual
            FacultyCard storedActualCard = new FacultyCard();
            storedActualCard.CopyCardInfo(ActualCard);

            // copy current card property to actual card
            ActualCard.CopyCardInfo(CurrentCard);

            bool success = FacultyServices.Instance.SaveFacultyCardToDatabase(ActualCard) && ActualCard.SaveTrainingFormOfFacultyListToDatabase();
            
            if (success)
            {
                // check if card exist -> Not exist insert new
                if (!isCardExist)
                {
                    AdminFacultyTrainingFormViewModel.StoredFacultyCards.Insert(0, ActualCard);
                    AdminFacultyTrainingFormViewModel.FacultyCards.Insert(0, ActualCard);
                    AdminFacultyTrainingFormViewModel.CurrentFacultyCards.Insert(0, ActualCard);
                    AdminFacultyTrainingFormViewModel.Instance.LoadFacultyByPageView();
                }
                MyMessageBox.Show("Thêm/chỉnh sửa khoa thành công");
            }
            else
            {
                // rollback to previous actual card
                ActualCard.CopyCardInfo(storedActualCard);
                MyMessageBox.Show("Có lỗi kết nối đến cơ sở dữ liệu, vui lòng thử lại sau");
            }

            ActualCard.RunOnPropertyChanged();
            ReturnToShowFacultyCardInfo();
        }

        public void ReturnToShowFacultyCardInfo()
        {
            AdminFacultyTrainingFormRightSideBarViewModel adminFacultyTrainingFormRightSideBarViewModel = AdminFacultyTrainingFormRightSideBarViewModel.Instance;
            adminFacultyTrainingFormRightSideBarViewModel.RightSideBarItemViewModel = new AdminFacultyRightSideBarItemViewModel(ActualCard);
        }

        public void AddToTrainingFormsListOfFacultyFunction(TrainingForm trainingForm)
        {
            CurrentCard.AddToTrainingFormOfFacultyList(trainingForm);
        }
        public void RemoveFromTrainingFormsListOfFacultyFunction(TrainingForm trainingForm)
        {
            CurrentCard.RemoveFromTrainingFormOfFacultyList(trainingForm);
        }
    }
}
