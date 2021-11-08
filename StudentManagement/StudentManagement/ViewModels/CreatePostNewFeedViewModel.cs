using Microsoft.Win32;
using StudentManagement.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class CreatePostNewFeedViewModel : BaseViewModel
    {
        public ICommand DeleteImage { get; set; }
        public ICommand AddImage { get; set; }
        public ICommand SendPost { get; set; }

        public string DraftPostText { get => _draftPostText; set { _draftPostText = value; OnPropertyChanged(); } }

        private bool _isPost;
        public bool IsPost { get => _isPost; set { _isPost = value; OnPropertyChanged(); } }

        public ObservableCollection<string> StackImageDraft { get => _stackImageDraft; set { _stackImageDraft = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _stackImageDraft;

        private string _draftPostText;

        public CreatePostNewFeedViewModel()
        {
            StackImageDraft = new ObservableCollection<string>();

            DeleteImage = new RelayCommand<object>(_ => true, (p) => DeleteImageInDraftPost(p));
            AddImage = new RelayCommand<object>(_ => true, _ => AddImageDraftPost());
            SendPost = new RelayCommand<object>(_ => true, _ => SendDraftPost());

        }

        private void DeleteImageInDraftPost(object image)
        {
            _ = StackImageDraft.Remove((image as Button)?.Tag as string);
        }

        private void SendDraftPost()
        {
            if (string.IsNullOrEmpty(DraftPostText) && StackImageDraft.Count == 0)
            {
                MyMessageBox.Show("Vui lòng nhập nội dung hoặc tải lên ảnh!", "Đăng bài không thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            IsPost = true;
        }

        private void AddImageDraftPost()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    StackImageDraft.Add(file);
                }
            }
        }
    }
}
