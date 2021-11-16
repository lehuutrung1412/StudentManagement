using StudentManagement.Commands;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels.SubjectClassDetail
{
    public class PostNewsfeedViewModel : BaseViewModel
    {
        private readonly CultureInfo _culture = new CultureInfo("vi-VN");
        public ICommand SendComment { get; set; }
        public ICommand ShowHideComments { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand DeleteComment { get; set; }
        public ICommand EditComment { get; set; }
        public ObservableCollection<PostComment> PostComments { get; set; }
        public bool IsShowComments { get => _isShowComments; set { _isShowComments = value; OnPropertyChanged(); } }
        private bool _isShowComments;

        private string _postText;
        public string PostText { get => _postText; set { _postText = value; OnPropertyChanged(); } }

        public DateTime PostTime { get; set; }

        private ObservableCollection<string> _stackPostImage;
        public string ImageSelectedShow { get => _imageSelectedShow; set { _imageSelectedShow = value; OnPropertyChanged(); } }

        private bool _isShowButtonChangeImage;

        private int _imageIndex;

        public Guid PostId { get; set; }
        public bool IsShowButtonChangeImage { get => _isShowButtonChangeImage; set { _isShowButtonChangeImage = value; OnPropertyChanged(); } }
        private string _imageSelectedShow;
        public ObservableCollection<string> StackPostImage
        {
            get => _stackPostImage;
            set
            {
                _stackPostImage = value;
                OnPropertyChanged();
                ImageSelectedShow = StackPostImage?.Count > 0 ? StackPostImage[0] : null;
                _imageIndex = ImageSelectedShow != null ? 0 : -1;
                IsShowButtonChangeImage = StackPostImage?.Count > 1;
            }
        }

        public PostNewsfeedViewModel(string postText, DateTime postTime, ObservableCollection<string> stackImage)
        {
            PostId = Guid.NewGuid();
            PostText = postText;
            PostTime = postTime; //new DateTime(2021, 11, 1, 20, 25, 30);
            IsShowComments = true;
            StackPostImage = new ObservableCollection<string>(stackImage);
            PostComments = new ObservableCollection<PostComment>();
            SendComment = new RelayCommand<object>((p) => true, (p) => SendDraftComment(p));
            ShowHideComments = new RelayCommand<object>((p) => true, (p) => ShowHideAllComments(p));
            ChangeImage = new RelayCommand<object>((p) => true, (p) => ChangeImageToShow(p));
            DeleteComment = new RelayCommand<Guid>((p) => true, (p) => DeleteOnComment(p));
            EditComment = new RelayCommand<object>((p) => true, (p) => EditOnComment(p));
        }

        private void SendDraftComment(object comment)
        {
            TextBox txbComment = comment as TextBox;
            if (txbComment.Text != "")
            {
                PostComments.Add(new PostComment(Guid.NewGuid(), "Lê Hữu Trung", txbComment.Text, DateTime.Parse(DateTime.Now.ToString(), _culture)));
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

        private void DeleteOnComment(Guid commentId)
        {
            try
            {
                PostComment commentToDelete = PostComments.Single(cmt => cmt.CommentId == commentId);
                if (MyMessageBox.Show("Bạn có chắc chắn muốn xóa bình luận này không?", "Xóa bình luận", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    _ = PostComments.Remove(commentToDelete);
                }
            }
            catch (Exception)
            {
                _ = MyMessageBox.Show("Đã có lỗi xảy ra, không thể xóa bình luận. Xin vui lòng thử lại", "Xóa bình luận", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void EditOnComment(object txbAndComment)
        {
            try
            {
                object[] values = (object[])txbAndComment;
                TextBox textBox = values[0] as TextBox;
                Guid commentId = (Guid)values[1];
                PostComment commentToEdit = PostComments.Single(cmt => cmt.CommentId == commentId);
                ControlTemplate template = textBox.Template;
                TextBox childTextBox = (TextBox)template.FindName("txbComment", textBox);
                childTextBox.Text = commentToEdit.Comment;
                childTextBox.CaretIndex = childTextBox.Text.Length;
                _ = childTextBox.Focus();
                _ = PostComments.Remove(commentToEdit);
            }
            catch (Exception)
            {
                _ = MyMessageBox.Show("Đã có lỗi xảy ra, không thể chỉnh sửa bình luận. Xin vui lòng thử lại", "Sửa bình luận", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }

    public class PostComment
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }

        public PostComment(Guid commentId, string username, string comment, DateTime time)
        {
            CommentId = commentId;
            Username = username;
            Comment = comment;
            Time = time;
        }
    }
}
