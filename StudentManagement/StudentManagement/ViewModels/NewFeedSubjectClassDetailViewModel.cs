using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class NewFeedSubjectClassDetailViewModel : BaseViewModel
    {
        public ICommand DeletePost { get; set; }

        private readonly CultureInfo _culture = new CultureInfo("vi-VN");

        public ObservableCollection<PostNewsfeedViewModel> PostNewsfeedViewModels { get; set; }

        public CreatePostNewFeedViewModel CreatePostNewFeedViewModel { get; set; }

        public NewFeedSubjectClassDetailViewModel()
        {
            CreatePostNewFeedViewModel = new CreatePostNewFeedViewModel();
            CreatePostNewFeedViewModel.PropertyChanged += CreatePostNewFeedViewModel_PropertyChanged;

            PostNewsfeedViewModels = new ObservableCollection<PostNewsfeedViewModel>();

            DeletePost = new RelayCommand<Guid>(_ => true, (p) => DeleteOnPost(p));
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

        private void CreatePostNewFeedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPost")
            {
                PostNewsfeedViewModels.Add(new PostNewsfeedViewModel(CreatePostNewFeedViewModel.DraftPostText, DateTime.Parse(DateTime.Now.ToString(), _culture), CreatePostNewFeedViewModel.StackImageDraft));
                CreatePostNewFeedViewModel.DraftPostText = "";
                CreatePostNewFeedViewModel.StackImageDraft.Clear();
            }
        }
    }
}
