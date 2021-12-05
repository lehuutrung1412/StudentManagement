using StudentManagement.Commands;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;
using StudentManagement.Services;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card

        public SubjectClassCard CurrentCard
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                OnPropertyChanged();
            }
        }

        private SubjectClassCard _currentCard;

        //store card info before edit
        private SubjectClassCard _actualCard;
        public SubjectClassCard ActualCard { get => _actualCard; set => _actualCard = value; }


        public AdminSubjectClassRightSideBarItemEditViewModel()
        {
            CurrentCard = null;
        }

        public AdminSubjectClassRightSideBarItemEditViewModel(SubjectClassCard card, bool isCreatedNew = false)
        {
            CurrentCard = new SubjectClassCard();
            ActualCard = card;
            if (!isCreatedNew)
            {
                CurrentCard.CopyCardInfo(card);
            }
            InitCommand();
        }

        public ICommand ConfirmEditSubjectClassCardInfo { get => _confirmEditSubjectClassCardInfo; set => _confirmEditSubjectClassCardInfo = value; }

        private ICommand _confirmEditSubjectClassCardInfo;

        public ICommand CancelEditSubjectClassCardInfo { get => _cancelEditSubjectClassCardInfo; set => _cancelEditSubjectClassCardInfo = value; }

        private ICommand _cancelEditSubjectClassCardInfo;

        public ICommand ClickChangeImageCommand { get; set; }

        public void InitCommand()
        {
            CancelEditSubjectClassCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditSubjectClassCardInfoFunction());
            ConfirmEditSubjectClassCardInfo = new RelayCommand<object>((p) =>
            {
                if (!CurrentCard.HasErrors && CanConfirmEdit())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }, (p) => ConfirmEditSubjectClassCardInfoFunction());
            ClickChangeImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickChangeImage());
        }

        public bool CanConfirmEdit()
        {
            if (!string.IsNullOrEmpty(CurrentCard.Code))

                return true;
            return false;
        }

        public void CancelEditSubjectClassCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowSubjectClassCardInfo();
        }

        public void ConfirmEditSubjectClassCardInfoFunction()
        {
            bool isCardExist = AdminSubjectClassViewModel.StoredSubjectClassCards.Contains(ActualCard);
            ActualCard.CopyCardInfo(CurrentCard);

            // check if card exist -> Not exist insert new
            if (!isCardExist)
            {
                AdminSubjectClassViewModel.StoredSubjectClassCards.Insert(0, ActualCard);
                AdminSubjectClassViewModel.SubjectClassCards.Insert(0, ActualCard);
            }

            SubjectClassServices.Instance.SaveSubjectClassCardToDatabase(ActualCard);


            ActualCard.RunOnPropertyChanged();
            ReturnToShowSubjectClassCardInfo();
        }

        public void ReturnToShowSubjectClassCardInfo()
        {
            AdminSubjectClassRightSideBarViewModel adminSubjectClassRightSideBarViewModel = AdminSubjectClassRightSideBarViewModel.Instance;
            adminSubjectClassRightSideBarViewModel.RightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemViewModel(ActualCard);
        }

        public void ClickChangeImage()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
            }
        }
    }

}
