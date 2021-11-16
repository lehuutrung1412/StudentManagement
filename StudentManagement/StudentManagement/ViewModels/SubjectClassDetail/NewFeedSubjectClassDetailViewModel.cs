using StudentManagement.Commands;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels.SubjectClassDetail
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

        public NewFeedSubjectClassDetailViewModel()
        {
            CreatePostNewFeedViewModel = new CreatePostNewFeedViewModel();
            CreatePostNewFeedViewModel.PropertyChanged += CreatePostNewFeedViewModel_PropertyChanged;
            EditPostNewFeedViewModel = new CreatePostNewFeedViewModel();
            EditPostNewFeedViewModel.PropertyChanged += EditPostNewFeedViewModel_PropertyChanged;

            PostNewsfeedViewModels = new ObservableCollection<PostNewsfeedViewModel>();

            DeletePost = new RelayCommand<Guid>(_ => true, (p) => DeleteOnPost(p));
            EditPost = new RelayCommand<UserControl>(_ => true, (p) => EditOnPost(p));
        }

        private void EditPostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                int index = PostNewsfeedViewModels.IndexOf(PostEditingViewModel);
                if (index > -1)
                {
                    PostEditingViewModel.PostText = EditPostNewFeedViewModel.DraftPostText;
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

        private void CreatePostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                PostNewsfeedViewModels.Add(new PostNewsfeedViewModel(CreatePostNewFeedViewModel.DraftPostText, DateTime.Parse(DateTime.Now.ToString(), _culture), CreatePostNewFeedViewModel.StackImageDraft));
                CreatePostNewFeedViewModel.DraftPostText = "";
                CreatePostNewFeedViewModel.StackImageDraft.Clear();
            }
        }

        private void DeleteOnPost(Guid postId)
        {
            try
            {
                PostNewsfeedViewModel postToDelete = PostNewsfeedViewModels.Single(vm => vm.PostId == postId);
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
            EditPostNewFeedViewModel.DraftPostText = editPostVM.PostText;
            EditPostNewFeedViewModel.StackImageDraft = new ObservableCollection<string>(editPostVM.StackPostImage);
            IsEditing = true;
        }
    }
}