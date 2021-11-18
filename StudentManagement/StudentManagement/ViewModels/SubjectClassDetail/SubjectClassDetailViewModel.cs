using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;

namespace StudentManagement.ViewModels
{
    public class SubjectClassDetailViewModel : BaseViewModel
    {
        private object _currentViewModel;
        private LayoutViewModel _layoutViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        // ViewModels -> To display view in layout
        private object _newFeedSubjectClassDetailViewModel;
        private object _fileManagerClassDetailViewModel;

        // Rightsidebar corresponding to _contentViewModel

        public SubjectClassDetailViewModel(UserControl cardComponent)
        {
            SubjectCard card = cardComponent.DataContext as SubjectCard;
            _layoutViewModel = new LayoutViewModel();

            InitContentView();
            InitRightSideBar();

            _layoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _newFeedSubjectClassDetailViewModel, null, _layoutViewModel, "Home"),
                new NavigationItem("Tài liệu", false, null, _fileManagerClassDetailViewModel, null, _layoutViewModel, "Home")
            };

            CurrentViewModel = _layoutViewModel;
        }

        public void InitContentView()
        {
            _newFeedSubjectClassDetailViewModel = new NewFeedSubjectClassDetailViewModel();
            _fileManagerClassDetailViewModel = new FileManagerClassDetailViewModel();

            _layoutViewModel.ContentViewModel = _newFeedSubjectClassDetailViewModel;
        }

        public void InitRightSideBar()
        {
        }
    }
}
