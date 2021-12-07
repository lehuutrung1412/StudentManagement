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
    public class NewFeedSubjectClassDetailViewModel : BaseViewModel
    {
        public ICommand DeletePost { get; set; }
        public ICommand EditPost { get; set; }

        private readonly CultureInfo _culture = new CultureInfo("vi-VN");

        public ObservableCollection<PostNewsfeedViewModel> PostNewsfeedViewModels { get; set; }
        public CreatePostNewFeedViewModel CreatePostNewFeedViewModel { get; set; }
        public CreatePostNewFeedViewModel EditPostNewFeedViewModel { get => _editPostNewFeedViewModel; set { _editPostNewFeedViewModel = value; OnPropertyChanged(); } }
        private CreatePostNewFeedViewModel _editPostNewFeedViewModel;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }
        private bool _isEditing;

        public PostNewsfeedViewModel PostEditingViewModel { get; set; }

        public SubjectClass SubjectClassDetail { get; set; }

        public NewFeedSubjectClassDetailViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;
            CreatePostNewFeedViewModel = new CreatePostNewFeedViewModel();
            CreatePostNewFeedViewModel.PropertyChanged += CreatePostNewFeedViewModel_PropertyChanged;
            EditPostNewFeedViewModel = new CreatePostNewFeedViewModel();
            EditPostNewFeedViewModel.PropertyChanged += EditPostNewFeedViewModel_PropertyChanged;
            DeletePost = new RelayCommand<object>(_ => true, (p) => DeleteOnPost(p));
            EditPost = new RelayCommand<UserControl>(_ => true, (p) => EditOnPost(p));

            LoadNewsfeed();

        }

        private void LoadNewsfeed()
        {
            PostNewsfeedViewModels = new ObservableCollection<PostNewsfeedViewModel>();
            var posts = NewsfeedServices.Instance.GetListNotificationOfSubjectClass(SubjectClassDetail.Id);
            foreach (var post in posts)
            {
                PostNewsfeedViewModels.Add(new PostNewsfeedViewModel(NewsfeedServices.Instance.ConvertNotificationToPostNewsfeed(post), new ObservableCollection<string>()));
            }
        }

        private void EditPostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                int index = PostNewsfeedViewModels.IndexOf(PostEditingViewModel);
                if (index > -1)
                {
                    PostEditingViewModel.Post.PostText = EditPostNewFeedViewModel.DraftPostText;
                    PostEditingViewModel.StackPostImage = new ObservableCollection<string>(EditPostNewFeedViewModel.StackImageDraft);
                    _ = MyMessageBox.Show("Chỉnh sửa bài đăng thành công!", "Sửa bài đăng", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
                else
                {
                    _ = MyMessageBox.Show("Đã có lỗi xảy ra! Xin vui lòng thử lại sau!", "Sửa bài đăng", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                IsEditing = false;
            }
        }

        private  void CreatePostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                try
                {
                    // Get current user
                    var user = UserServices.Instance.GetUserInfo();

                    NewsfeedPost post = new NewsfeedPost()
                    {
                        PostId = Guid.NewGuid(),
                        IdPoster = user.Id,
                        IdSubjectClass = SubjectClassDetail.Id,
                        PosterName = user.DisplayName,
                        PostText = CreatePostNewFeedViewModel.DraftPostText,
                        PostTime = DateTime.Parse(DateTime.Now.ToString(), _culture),
                        Topic = SubjectClassDetail.Code 
                        //+ " - " + SubjectClassDetail.Subject.DisplayName
                    };
                    PostNewsfeedViewModels.Add(new PostNewsfeedViewModel(post, CreatePostNewFeedViewModel.StackImageDraft));

                    NewsfeedServices.Instance.SavePostToDatabaseAsync(post);

                    CreatePostNewFeedViewModel.DraftPostText = "";

                    CreatePostNewFeedViewModel.StackImageDraft.Clear();
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Đăng tin không thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }

            }
        }

        private void DeleteOnPost(object p)
        {
            try
            {
                Guid? postId = p as Guid?;
                PostNewsfeedViewModel postToDelete = PostNewsfeedViewModels.Single(vm => vm.Post.PostId == postId);
                if (MyMessageBox.Show("Bạn có chắc chắn muốn xóa bài đăng này không?", "Xóa bài đăng", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    _ = PostNewsfeedViewModels.Remove(postToDelete);
                }
            }
            catch (Exception)
            {
                _ = MyMessageBox.Show("Đã có lỗi xảy ra, không thể xóa bài đăng. Xin vui lòng thử lại", "Xóa bài đăng", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void EditOnPost(UserControl post)
        {
            PostNewsfeedViewModel editPostVM = post.DataContext as PostNewsfeedViewModel;
            PostEditingViewModel = editPostVM;
            EditPostNewFeedViewModel.DraftPostText = editPostVM.Post.PostText;
            EditPostNewFeedViewModel.StackImageDraft = new ObservableCollection<string>(editPostVM.StackPostImage);
            IsEditing = true;
        }
    }
}