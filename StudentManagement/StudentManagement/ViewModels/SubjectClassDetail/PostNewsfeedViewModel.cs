using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class PostNewsfeedViewModel : BaseViewModel
    {
        private readonly CultureInfo _culture = new CultureInfo("vi-VN");
        public ICommand SendComment { get; set; }
        public ICommand ShowHideComments { get; set; }
        public ICommand ChangeImage { get; set; }
        public ICommand DeleteComment { get; set; }
        public ICommand EditComment { get; set; }
        
        public bool IsShowComments { get => _isShowComments; set { _isShowComments = value; OnPropertyChanged(); } }
        private bool _isShowComments;

        private ObservableCollection<string> _stackPostImage;
        public string ImageSelectedShow { get => _imageSelectedShow; set { _imageSelectedShow = value; OnPropertyChanged(); } }

        private bool _isShowButtonChangeImage;

        private int _imageIndex;
        
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

        public NewsfeedPost Post { get; set; }

        public ObservableCollection<PostComment> PostComments { get; set; }

        public PostNewsfeedViewModel(NewsfeedPost post, ObservableCollection<string> stackImage)
        {
            Post = post;
            IsShowComments = true;
            StackPostImage = new ObservableCollection<string>(stackImage);


            SendComment = new RelayCommand<object>((p) => true, (p) => SendDraftComment(p));
            ShowHideComments = new RelayCommand<object>((p) => true, (p) => ShowHideAllComments(p));
            ChangeImage = new RelayCommand<object>((p) => true, (p) => ChangeImageToShow(p));
            DeleteComment = new RelayCommand<Guid?>((p) => true, (p) => DeleteOnComment(p));
            EditComment = new RelayCommand<object>((p) => true, (p) => EditOnComment(p));

            FirstLoadComment();
        }

        private void FirstLoadComment()
        {
            try
            {
                PostComments = new ObservableCollection<PostComment>();
                var comments = NewsfeedServices.Instance.GetListCommentInPost(Post.PostId);
                foreach (var comment in comments)
                {
                    PostComments.Add(NewsfeedServices.Instance.ConvertNotificationCommentToPostComment(comment));
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải bình luận!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void SendDraftComment(object comment)
        {
            try
            {
                TextBox txbComment = comment as TextBox;
                if (txbComment.Text != "")
                {
                    // Get current user
                    var user = LoginServices.CurrentUser;

                    var newComment = new PostComment(Guid.NewGuid(), Post.PostId, user.Id, user.IdAvatar != null ? user.DatabaseImageTable.Image : null, user.DisplayName, txbComment.Text, DateTime.Parse(DateTime.Now.ToString(), _culture));

                    await NewsfeedServices.Instance.SaveCommentToDatabaseAsync(newComment);
                    await NewsfeedServices.Instance.SaveCommentToNotification(newComment);
                    await NewsfeedServices.Instance.SaveCommentToNotificationInfo(newComment);

                    PostComments.Add(newComment);
                    txbComment.Text = "";
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể đăng bình luận!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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

        private async void DeleteOnComment(Guid? commentId)
        {
            try
            {
                PostComment commentToDelete = PostComments.Single(cmt => cmt.Id == commentId);
                if (MyMessageBox.Show("Bạn có chắc chắn muốn xóa bình luận này không?", "Xóa bình luận", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    await NewsfeedServices.Instance.DeleteCommentAsync(commentId);
                    _ = PostComments.Remove(commentToDelete);
                }
            }
            catch (Exception)
            {
                _ = MyMessageBox.Show("Đã có lỗi xảy ra, không thể xóa bình luận. Xin vui lòng thử lại", "Xóa bình luận", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void EditOnComment(object txbAndComment)
        {
            try
            {
                object[] values = (object[])txbAndComment;
                TextBox textBox = values[0] as TextBox;
                Guid commentId = (Guid)values[1];
                PostComment commentToEdit = PostComments.Single(cmt => cmt.Id == commentId);
                ControlTemplate template = textBox.Template;
                TextBox childTextBox = (TextBox)template.FindName("txbComment", textBox);
                childTextBox.Text = commentToEdit.Comment;
                childTextBox.CaretIndex = childTextBox.Text.Length;
                _ = childTextBox.Focus();

                await NewsfeedServices.Instance.DeleteCommentAsync(commentId);
                _ = PostComments.Remove(commentToEdit);
            }
            catch (Exception)
            {
                _ = MyMessageBox.Show("Đã có lỗi xảy ra, không thể chỉnh sửa bình luận. Xin vui lòng thử lại", "Sửa bình luận", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
