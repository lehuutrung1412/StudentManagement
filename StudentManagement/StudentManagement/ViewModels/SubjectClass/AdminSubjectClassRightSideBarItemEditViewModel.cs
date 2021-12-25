using StudentManagement.Commands;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;
using StudentManagement.Services;
using System.Windows.Forms;

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
            card.InitCardData();
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
            if (string.IsNullOrEmpty(CurrentCard.Code))
                return false;
            if (!CurrentCard.StartDate.HasValue)
                return false;
            if (!CurrentCard.EndDate.HasValue)
                return false;
            if (String.IsNullOrEmpty(CurrentCard.MaxNumberOfStudents.ToString()))
                return false;
            if (CurrentCard.SelectedSubject == null)
                return false;
            if (CurrentCard.SelectedTrainingForm == null)
                return false;
            if (CurrentCard.SelectedDay == null)
                return false;
            if (CurrentCard.SelectedSemester == null)
                return false;
            if (CurrentCard.SelectedSubject == null)
                return false;
            return true;
        }

        public void CancelEditSubjectClassCardInfoFunction()
        {
            CurrentCard.CopyCardInfo(ActualCard);
            ReturnToShowSubjectClassCardInfo();
        }

        public async void ConfirmEditSubjectClassCardInfoFunction()
        {
            bool isCardExist = AdminSubjectClassViewModel.StoredSubjectClassCards.Contains(ActualCard);

            // store current actual
            SubjectClassCard storedActualCard = new SubjectClassCard();
            storedActualCard.CopyCardInfo(ActualCard);

            // copy current card property to actual card
            var tmpImage = ActualCard.Image;
            ActualCard.CopyCardInfo(CurrentCard);

            if (tmpImage != null && !ActualCard.Image.Equals(tmpImage))
            {
                var uploadImageTasks = new List<Task<string>>();
                uploadImageTasks.Add(ImageUploader.Instance.UploadAsync(ActualCard.Image));
                foreach (var img in await Task.WhenAll(uploadImageTasks))
                {
                    ActualCard.Image = img;
                }
            }

            bool success = await SubjectClassServices.Instance.SaveSubjectClassCardToDatabase(ActualCard);

            if (success)
            {
                // check if card exist -> Not exist insert new
                if (!isCardExist)
                {
                    AdminSubjectClassViewModel.StoredSubjectClassCards.Insert(0, ActualCard);
                    AdminSubjectClassViewModel.SubjectClassCards.Insert(0, ActualCard);
                }
                MyMessageBox.Show("Thêm/chỉnh sửa lớp môn học thành công");
            }
            else
            {
                // rollback to previous actual card
                ActualCard.CopyCardInfo(storedActualCard);
                MyMessageBox.Show("Có lỗi kết nối đến cơ sở dữ liệu, vui lòng thử lại sau");
            }


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
            if (op.ShowDialog() == DialogResult.OK)
            {
                CurrentCard.Image = op.FileName;
                
            }
        }
    }

}
