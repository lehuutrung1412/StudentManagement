using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

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
            DeletePost = new RelayCommand<Guid?>(_ => true, (p) => DeleteOnPost(p));
            EditPost = new RelayCommand<UserControl>(_ => true, (p) => EditOnPost(p));

            LoadNewsfeed();

            DeletePost = new RelayCommand<Guid?>((p) => true, (p) => DeleteOnPost(p));
            EditPost = new RelayCommand<UserControl>((p) => true, (p) => EditOnPost(p));
        }

        private void LoadNewsfeed()
        {
            try
            {
                PostNewsfeedViewModels = new ObservableCollection<PostNewsfeedViewModel>();
                var posts = NewsfeedServices.Instance.GetListNotificationOfSubjectClass(SubjectClassDetail.Id);
                foreach (var post in posts)
                {
                    // Load image
                    var images = new ObservableCollection<string>(NewsfeedServices.Instance.GetListImagesInPost(post.Id));

                    PostNewsfeedViewModels.Add(new PostNewsfeedViewModel(NewsfeedServices.Instance.ConvertNotificationToPostNewsfeed(post), images));
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải bài đăng!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void EditPostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                try
                {
                    int index = PostNewsfeedViewModels.IndexOf(PostEditingViewModel);
                    if (index > -1)
                    {
                        PostEditingViewModel.Post.PostText = EditPostNewFeedViewModel.DraftPostText;

                        await NewsfeedServices.Instance.SavePostToDatabaseAsync(PostEditingViewModel.Post);

                        // Upload Image
                        var stackImageUploaded = new ObservableCollection<string>();
                        var uploadImageTasks = new List<Task<string>>();

                        Parallel.ForEach(EditPostNewFeedViewModel.StackImageDraft, img =>
                        {
                            if (!img.Contains("http"))
                            {
                                uploadImageTasks.Add(ImageUploader.Instance.UploadAsync(img));
                            }
                            else
                            {
                                stackImageUploaded.Add(img);
                            }
                        });

                        foreach (var img in await Task.WhenAll(uploadImageTasks))
                        {
                            await NewsfeedServices.Instance.SaveImageToDatabaseAsync(PostEditingViewModel.Post.PostId, img);

                            stackImageUploaded.Add(img);
                        }

                        await NewsfeedServices.Instance.DeleteImagesInPostAsync(PostEditingViewModel.Post.PostId, PostEditingViewModel.StackPostImage.Except(EditPostNewFeedViewModel.StackImageDraft).ToList());

                        PostEditingViewModel.StackPostImage = stackImageUploaded;

                        MainWindow.Notify.ShowBalloonTip(3000, "Sửa bài đăng", "Chỉnh sửa bài đăng thành công!", ToolTipIcon.Info);

                        //_ = MyMessageBox.Show("Chỉnh sửa bài đăng thành công!", "Sửa bài đăng", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                    else
                    {
                        _ = MyMessageBox.Show("Đã có lỗi xảy ra! Xin vui lòng thử lại sau!", "Sửa bài đăng", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                    IsEditing = false;
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Đã có lỗi xảy ra! Không thể sửa bài đăng!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private async void CreatePostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                try
                {
                    // Get current user
                    var user = LoginServices.CurrentUser;

                    NewsfeedPost post = new NewsfeedPost()
                    {
                        PostId = Guid.NewGuid(),
                        IdPoster = user.Id,
                        IdSubjectClass = SubjectClassDetail.Id,
                        PosterName = user.DisplayName,
                        PosterAvatar = user.IdAvatar != null ? user.DatabaseImageTable.Image : null,
                        PostText = CreatePostNewFeedViewModel.DraftPostText,
                        PostTime = DateTime.Parse(DateTime.Now.ToString(), _culture),
                        Topic = SubjectClassDetail.Code + " - " + SubjectClassDetail.Subject.DisplayName
                    };

                    await NewsfeedServices.Instance.SavePostToDatabaseAsync(post);
                    await NewsfeedServices.Instance.SavePostToNotificationInfoAsync(post);

                    // Upload image
                    MainWindow.Notify.ShowBalloonTip(3000, "Đang tải lên...", "Vui lòng chờ chút bạn nhé!", ToolTipIcon.Info);

                    var stackImageUploaded = new ObservableCollection<string>();
                    var uploadImageTasks = new List<Task<string>>();

                    Parallel.ForEach(CreatePostNewFeedViewModel.StackImageDraft, img =>
                    {
                        uploadImageTasks.Add(ImageUploader.Instance.UploadAsync(img));
                    });

                    foreach (var img in await Task.WhenAll(uploadImageTasks))
                    {
                        await NewsfeedServices.Instance.SaveImageToDatabaseAsync(post.PostId, img);

                        stackImageUploaded.Add(img);
                    }

                    PostNewsfeedViewModels.Add(new PostNewsfeedViewModel(post, stackImageUploaded));
                    CreatePostNewFeedViewModel.DraftPostText = "";

                    CreatePostNewFeedViewModel.StackImageDraft.Clear();

                    MainWindow.Notify.ShowBalloonTip(3000, post.Topic, post.PostText, ToolTipIcon.Info);

                }
                catch (Exception)
                {
                    MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Đăng tin không thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }

            }
        }

        private async void DeleteOnPost(Guid? postId)
        {
            try
            {
                PostNewsfeedViewModel postToDelete = PostNewsfeedViewModels.Single(vm => vm.Post.PostId == postId);
                if (MyMessageBox.Show("Bạn có chắc chắn muốn xóa bài đăng này không?", "Xóa bài đăng", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    await NewsfeedServices.Instance.DeletePostAsync(postId);
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