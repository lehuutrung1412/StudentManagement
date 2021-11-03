using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class PostNewsfeedViewModel : BaseViewModel
    {
        public ICommand SendComment { get; set; }
        public ICommand ShowHideComments { get; set; }
        public ICommand ChangeImage { get; set; }
        public ObservableCollection<PostComment> PostComments { get; set; }
        public bool IsShowComments { get => _isShowComments; set { _isShowComments = value; OnPropertyChanged(); } }
        private bool _isShowComments;

        private string _postText;
        public string PostText { get => _postText; set { _postText = value; OnPropertyChanged(); } }

        public DateTime PostTime { get; set; }

        public ObservableCollection<string> StackPostImage { get; set; }
        public string ImageSelectedShow { get => _imageSelectedShow; set { _imageSelectedShow = value; OnPropertyChanged(); } }

        private string _imageSelectedShow;

        public bool IsShowButtonChangeImage { get; set; }

        private int _imageIndex;

        public PostNewsfeedViewModel(string postText, DateTime postTime, ObservableCollection<string> stackImage)
        {
            PostText = postText;
            PostTime = postTime; //new DateTime(2021, 11, 3, 20, 25, 30);
            IsShowComments = true;
            StackPostImage = new ObservableCollection<string>(stackImage);
            ImageSelectedShow = StackPostImage?.Count > 0 ? StackPostImage[0] : null;
            _imageIndex = ImageSelectedShow != null ? 0 : -1;
            IsShowButtonChangeImage = StackPostImage?.Count > 1;
            PostComments = new ObservableCollection<PostComment>();
            SendComment = new RelayCommand<object>(_ => true, (p) => SendDraftComment(p));
            ShowHideComments = new RelayCommand<object>(_ => true, (p) => ShowHideAllComments(p));
            ChangeImage = new RelayCommand<object>(_ => true, (p) => ChangeImageToShow(p));
        }

        private void SendDraftComment(object comment)
        {
            TextBox txbComment = comment as TextBox;
            if (txbComment.Text != "")
            {
                PostComments.Add(new PostComment("Lê Hữu Trung", txbComment.Text));
                txbComment.Text = "";
            }
        }

        private void ShowHideAllComments(object allComments)
        {
            IsShowComments = !IsShowComments;
            ItemsControl icComments = allComments as ItemsControl;
            icComments.Visibility = IsShowComments ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void ChangeImageToShow(object arrowButton)
        {
            _imageIndex = (arrowButton as Image)?.Name == "leftArrow"
                ? _imageIndex - 1 >= 0 ? _imageIndex - 1 : 0
                : _imageIndex + 1 < StackPostImage.Count ? _imageIndex + 1 : StackPostImage.Count - 1;
            ImageSelectedShow = StackPostImage[_imageIndex];
        }
    }

    public class PostComment
    {
        public string Username { get; set; }
        public string Comment { get; set; }

        public PostComment(string username, string comment)
        {
            Username = username;
            Comment = comment;
        }
    }
}
