using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;

namespace StudentManagement.ViewModels
{
    public class SubjectClassDetailViewModel : BaseViewModel
    {
        // ViewModels -> To display view in layout
        private LayoutViewModel _layoutViewModel;
        private object _newFeedSubjectClassDetailViewModel;
        private object _fileManagerClassDetailViewModel;

        // Rightsidebar corresponding to _contentViewModel

        // Properties
        private bool _isShowDialog;
        public bool IsShowDialog { get => _isShowDialog; set { _isShowDialog = value; OnPropertyChanged(); } }

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public object DialogViewModel { get => _dialogViewModel; set { _dialogViewModel = value; OnPropertyChanged(); } }

        private object _dialogViewModel;

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

        #region Methods

        public void InitContentView()
        {
            _newFeedSubjectClassDetailViewModel = new NewFeedSubjectClassDetailViewModel();
            (_newFeedSubjectClassDetailViewModel as NewFeedSubjectClassDetailViewModel).PropertyChanged += NewFeedSubjectClassDetailViewModel_PropertyChanged;
            _fileManagerClassDetailViewModel = new FileManagerClassDetailViewModel();
            (_fileManagerClassDetailViewModel as FileManagerClassDetailViewModel).PropertyChanged += FileManagerClassDetailViewModel_PropertyChanged;
            _layoutViewModel.ContentViewModel = _newFeedSubjectClassDetailViewModel;
        }

        private void NewFeedSubjectClassDetailViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEditing")
            {
                IsShowDialog = (_newFeedSubjectClassDetailViewModel as NewFeedSubjectClassDetailViewModel).IsEditing;
                if (IsShowDialog)
                {
                    DialogViewModel = (_newFeedSubjectClassDetailViewModel as NewFeedSubjectClassDetailViewModel).EditPostNewFeedViewModel;
                }
            }
        }

        public void InitRightSideBar()
        {
        }

        #endregion Methods

        #region Events
        private void FileManagerClassDetailViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsShowDialog")
            {
                IsShowDialog = (_fileManagerClassDetailViewModel as FileManagerClassDetailViewModel).IsShowDialog;
                if (IsShowDialog)
                {
                    DialogViewModel = _fileManagerClassDetailViewModel;
                }
            }
        }
        #endregion Events
    }
}
