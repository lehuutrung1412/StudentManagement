using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
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
        private object _adminStudentListViewModel;
        private object _settingSubjectClassDetailViewModel;

        // Rightsidebar corresponding to _contentViewModel
        private object _newsFeedRightSideBarViewModel;
        private object _fileManagerRightSideBarViewModel;
        private object _studentListRightSideBar;

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

        public SubjectClassDetailViewModel(SubjectClassCard card)
        {
            //SubjectClassCard card = cardComponent.DataContext as SubjectClassCard;
            var subjectClass = SubjectClassServices.Instance.FindSubjectClassBySubjectClassId(card.Id);

            _layoutViewModel = new LayoutViewModel();

            InitContentView(subjectClass);
            InitRightSideBar(subjectClass);

            _layoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Bảng tin", false, null, _newFeedSubjectClassDetailViewModel, _newsFeedRightSideBarViewModel, _layoutViewModel, "NewspaperVariantOutline"),
                new NavigationItem("Tài liệu", false, null, _fileManagerClassDetailViewModel, _fileManagerRightSideBarViewModel, _layoutViewModel, "FileDocumentMultipleOutline"),
                new NavigationItem("Danh sách sinh viên", false, null, _adminStudentListViewModel, _studentListRightSideBar, _layoutViewModel, "SchoolOutline"),
            };

            if (LoginServices.CurrentUser.UserRole.Role == "Admin" || LoginServices.CurrentUser.UserRole.Role == "Giáo viên")
            {
                _layoutViewModel.NavigationItems.Add(new NavigationItem("Cài đặt", false, null, _settingSubjectClassDetailViewModel, null, _layoutViewModel, "CogOutline"));
            }

            // Set corresponding active button to default view
            ObservableCollection<NavigationItem> navigationItems = _layoutViewModel.NavigationItems;
            foreach (var item in navigationItems)
            {
                if (item.NavigationItemViewModel == _layoutViewModel.ContentViewModel)
                {
                    item.IsPressed = true;
                    break;
                }
            }

            CurrentViewModel = _layoutViewModel;
        }

        #region Methods

        public void InitContentView(SubjectClass subjectClass)
        {
            _newFeedSubjectClassDetailViewModel = new NewFeedSubjectClassDetailViewModel(subjectClass);
            (_newFeedSubjectClassDetailViewModel as NewFeedSubjectClassDetailViewModel).PropertyChanged += NewFeedSubjectClassDetailViewModel_PropertyChanged;


            _fileManagerClassDetailViewModel = new FileManagerClassDetailViewModel(subjectClass);
            (_fileManagerClassDetailViewModel as FileManagerClassDetailViewModel).PropertyChanged += FileManagerClassDetailViewModel_PropertyChanged;
            
            _adminStudentListViewModel = new AdminStudentListViewModel(subjectClass);
            (_adminStudentListViewModel as AdminStudentListViewModel).PropertyChanged += StudentListViewModel_PropertyChanged;

            _settingSubjectClassDetailViewModel = new SettingSubjectClassDetailViewModel(subjectClass);

            _layoutViewModel.ContentViewModel = _newFeedSubjectClassDetailViewModel;
        }


        public void InitRightSideBar(SubjectClass subjectClass)
        {
            _fileManagerRightSideBarViewModel = new FileManagerRightSideBarViewModel();
            (_fileManagerRightSideBarViewModel as FileManagerRightSideBarViewModel).PropertyChanged += FileManagerRightSideBarViewModel_PropertyChanged;
            
            _newsFeedRightSideBarViewModel = new NewsfeedRightSideBarViewModel(subjectClass);

            _studentListRightSideBar = new StudentListRightSideBarViewModel(subjectClass);
            (_studentListRightSideBar as StudentListRightSideBarViewModel).PropertyChanged += StudentListRightSideBarViewModel_PropertyChanged;

            _layoutViewModel.RightSideBar = _newsFeedRightSideBarViewModel;
        }


        #endregion Methods

        #region Events

        private void StudentListRightSideBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ReloadData")
            {
                (_adminStudentListViewModel as AdminStudentListViewModel).FirstLoadData();
                (_adminStudentListViewModel as AdminStudentListViewModel).DrawPieChart();
            }
        }

        private void FileManagerRightSideBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDeleteFile")
            {
                object selectedItems = new ObservableCollection<FileInfo>
                    { (_fileManagerRightSideBarViewModel as FileManagerRightSideBarViewModel).CurrentFile };
                (_fileManagerClassDetailViewModel as FileManagerClassDetailViewModel).DeleteFileFunction(selectedItems);
            }
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
            else if (e.PropertyName == "SelectedFile")
            {
                object selectedFile = (_fileManagerClassDetailViewModel as FileManagerClassDetailViewModel).SelectedFile;
                (_fileManagerRightSideBarViewModel as FileManagerRightSideBarViewModel).CurrentFile = selectedFile as FileInfo;
            }
        }

        private void StudentListViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                object selectedItem = (_adminStudentListViewModel as AdminStudentListViewModel).SelectedItem;
                (_studentListRightSideBar as StudentListRightSideBarViewModel).SelectedItem = selectedItem as StudentGrid;
            }
        }
        #endregion Events
    }
}
