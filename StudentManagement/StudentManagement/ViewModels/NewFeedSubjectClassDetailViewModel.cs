using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class NewFeedSubjectClassDetailViewModel : BaseViewModel
    {
        private readonly CultureInfo _culture = new CultureInfo("vi-VN");

        public ObservableCollection<PostNewsfeedViewModel> PostNewsfeedViewModels { get; set; }

        public CreatePostNewFeedViewModel CreatePostNewFeedViewModel { get; set; }

        public NewFeedSubjectClassDetailViewModel()
        {
            CreatePostNewFeedViewModel = new CreatePostNewFeedViewModel();
            CreatePostNewFeedViewModel.PropertyChanged += CreatePostNewFeedViewModel_PropertyChanged;

            PostNewsfeedViewModels = new ObservableCollection<PostNewsfeedViewModel>();
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
