using StudentManagement.Commands;
using StudentManagement.Components;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class LayoutViewModel : BaseViewModel
    {
        // current contentViewModel and rightSideBarViewModel
        private object _contentViewModel;
        private object _rightSideBar;


        // ViewModels -> To display view in layout
        private object _adminHomeViewModel;
        private object _adminSubjectClassViewModel;
        private object _adminNotificationViewModel;
        private object _newFeedSubjectClassDetailViewModel;
        private object _studentCourseRegistryViewModel;
        private object _adminFalcutyTrainingFormViewModel;
        private object _scoreboardViewModel;

        // Rightsidebar corresponding to _contentViewModel
        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;
        private object _adminNotificationRightSideBar;
        private object _studentCourseRegistryRightSideBar;
        private object _adminFalcutyTrainingFormRightSideBar;
        private object _scoreboardRightSideBar;

        public object ContentViewModel
        {
            get { return _contentViewModel; }
            set
            {
                _contentViewModel = value;
                OnPropertyChanged();
            }
        }

        public object RightSideBar
        {
            get { return _rightSideBar; }
            set
            {
                _rightSideBar = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NavigationItem> _navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems { get => _navigationItems; set => _navigationItems = value; }
        public object NewFeedSubjectClassDetailViewModel { get => _newFeedSubjectClassDetailViewModel; set => _newFeedSubjectClassDetailViewModel = value; }

        public LayoutViewModel()
        {
            InitContentView();

            InitRightSideBar();


            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, this, "School"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFalcutyTrainingFormViewModel, _adminFalcutyTrainingFormRightSideBar, this, "School")
            };

            NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Trang chủ", false, null, _adminHomeViewModel, _adminHomeRightSideBar, this, "Home"),
                new NavigationItem("Lớp môn học", false, null, _newFeedSubjectClassDetailViewModel, null, this, "Home"),
                new NavigationItem("Đào tạo", true, temp, null, null, this, "ClockOutline"),
                new NavigationItem("Đăng ký học phần", false, null, _studentCourseRegistryViewModel, _studentCourseRegistryRightSideBar, this, "CreditCardPlusOutline"),
                new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, this, "Cat"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, this, "Home"),
            };

        }

        public void InitContentView()
        {
            this._adminHomeViewModel = new AdminHomeViewModel();

            this._adminSubjectClassViewModel = new AdminSubjectClassViewModel();

            this._adminNotificationViewModel = new AdminNotificationViewModel();

            this._newFeedSubjectClassDetailViewModel = new NewFeedSubjectClassDetailViewModel();

            this._studentCourseRegistryViewModel = new StudentCourseRegistryViewModel();

            this._adminFalcutyTrainingFormViewModel = new AdminFalcutyTrainingFormViewModel();

            this._studentCourseRegistryViewModel = new StudentCourseRegistryViewModel();

            this._scoreboardViewModel = new ScoreBoardViewModel();

            

            this.ContentViewModel = this._adminHomeViewModel;
        }

        public void InitRightSideBar()
        {
            this._adminHomeRightSideBar = new AdminHomeRightSideBarViewModel();

            this._adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBarViewModel();

            this._adminNotificationRightSideBar = new AdminNotificationRightSideBarViewModel();

            this._studentCourseRegistryRightSideBar = new StudentCourseRegistryRightSideBarViewModel();

            this._adminFalcutyTrainingFormRightSideBar = new AdminFalcutyTrainingFormRightSideBarViewModel();

            this._scoreboardRightSideBar = new ScoreBoardRightSideBarViewModel(); //ViewModel

            this.RightSideBar = this._adminHomeRightSideBar;
        }

        public class NavigationItem
        {
            private string _navigationHeader;

            private bool _canBeExpanded;

            private ObservableCollection<NavigationItem> _expandedItems;

            private object _navigationItemViewModel;

            private object _rightSideBarNavigationItemViewModel;

            private ICommand _goToView;

            private static LayoutViewModel _layoutViewModel;

            private string _icon;

            public NavigationItem(string navigationHeader, bool canBeExpanded, ObservableCollection<NavigationItem> expandedItems, object navigationItemViewModel, object rightSideBarNavigationItemViewModel, LayoutViewModel layoutViewModel, string icon)
            {
                _navigationHeader = navigationHeader;
                _canBeExpanded = canBeExpanded;
                _expandedItems = expandedItems;
                _navigationItemViewModel = navigationItemViewModel;
                _rightSideBarNavigationItemViewModel = rightSideBarNavigationItemViewModel;
                LayoutViewModel = layoutViewModel;
                _icon = icon;
                GoToView = new RelayCommand<object>((p) => { return true; }, (p) => GoToViewFunction());
            }

            public string NavigationHeader { get => _navigationHeader; set => _navigationHeader = value; }
            public bool CanBeExpanded { get => _canBeExpanded; set => _canBeExpanded = value; }
            public ObservableCollection<NavigationItem> ExpandedItems { get => _expandedItems; set => _expandedItems = value; }
            public object NavigationItemViewModel { get => _navigationItemViewModel; set => _navigationItemViewModel = value; }
            public object RightSideBarNavigationItemViewModel { get => _rightSideBarNavigationItemViewModel; set => _rightSideBarNavigationItemViewModel = value; }
            public ICommand GoToView { get => _goToView; set => _goToView = value; }
            public static LayoutViewModel LayoutViewModel { get => _layoutViewModel; set => _layoutViewModel = value; }
            public string Icon { get => _icon; set => _icon = value; }

            private void GoToViewFunction()
            {
                LayoutViewModel.ContentViewModel = this.NavigationItemViewModel;
                LayoutViewModel.RightSideBar = this.RightSideBarNavigationItemViewModel;

            }

        }
    }
}
