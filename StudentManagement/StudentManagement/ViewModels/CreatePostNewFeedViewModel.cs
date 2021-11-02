using Microsoft.Win32;
using StudentManagement.Commands;
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

        public ObservableCollection<string> StackImageDraft { get; set; }

        private string _draftPostText;

        public CreatePostNewFeedViewModel()
        {
            StackImageDraft = new ObservableCollection<string>();

            DeleteImage = new RelayCommand<object>(_ => true, (p) => DeleteImageInDraftPost(p));
            AddImage = new RelayCommand<object>(_ => true, _ => AddImageDraftPost());
            SendPost = new RelayCommand<UserControl>(_ => true, (p) => SendDraftPost(p));

        }

        private void DeleteImageInDraftPost(object image)
        {
            _ = StackImageDraft.Remove((image as Button)?.Tag as string);
        }

        private void SendDraftPost(UserControl post)
        {
            MyMessageBox.Show(DraftPostText);
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
