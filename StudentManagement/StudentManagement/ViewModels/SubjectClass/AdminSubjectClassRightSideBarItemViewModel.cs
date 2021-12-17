using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;
using Microsoft.Win32;
using StudentManagement.Objects;
using StudentManagement.Services;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassRightSideBarItemViewModel : BaseViewModel
    {
        public SubjectClassCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private SubjectClassCard _currentCard;

        public ICommand ClickChangeImageCommand { get; set; }

        public AdminSubjectClassRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public  AdminSubjectClassRightSideBarItemViewModel(SubjectClassCard card)
        {
            CurrentCard = card;

            ClickChangeImageCommand = new RelayCommand<object>(
            (p) => { return true; },
            async (p) =>
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                };
                if (op.ShowDialog() == true)
                {
                    try
                    {
                        CurrentCard.Image = op.FileName;
                        var uploadImageTasks = new List<Task<string>>();
                        uploadImageTasks.Add(ImageUploader.Instance.UploadAsync(CurrentCard.Image));
                        foreach (var img in await Task.WhenAll(uploadImageTasks))
                        {
                            CurrentCard.Image = img;
                        }
                        await SubjectClassServices.Instance.SaveSubjectClassCardToDatabase(CurrentCard);
                        CurrentCard.RunOnPropertyChanged();
                    }
                    catch 
                    {
                        MyMessageBox.Show("Đã có lỗi trong cập nhật ảnh đại diện","Thông báo",System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
               
                }
            });
        }
    }
}
