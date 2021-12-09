using StudentManagement.Commands;
using StudentManagement.Components;
using StudentManagement.Objects;
using StudentManagement.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using NavigationItem = StudentManagement.Objects.NavigationItem;

namespace StudentManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentViewModel;
        private LoginViewModel _loginViewModel;
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

        public ICommand GotoLoginViewCommand { get; set; }
        public ICommand GotoLayoutViewCommand { get; set; }

        // ViewModels -> To display view in layout
        private object _adminHomeViewModel;
        private object _adminSubjectClassViewModel;
        private object _adminSubjectViewModel;
        private object _adminNotificationViewModel;
        private object _studentCourseRegistryViewModel;
        private object _adminFacultyTrainingFormViewModel;
        private object _scoreboardViewModel;
        private object _studentScheduleTableViewModel;
        private object _adminUserInfoViewModel;
        private object _adminCourseRegistryViewModel;
        private object _adminStudentListViewModel;
        private object _settingUserInfoViewModel;
        private object _campusStudentListViewModel;

        // Rightsidebar corresponding to _contentViewModel
        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;
        private object _adminSubjectRightSideBar;
        private object _adminNotificationRightSideBar;
        private object _studentCourseRegistryRightSideBar;
        private object _adminFacultyTrainingFormRightSideBar;
        private object _scoreboardRightSideBar;
        private object _adminCourseRegistryRightSideBar;
        private object _studentListRightSideBar;
        private object _campusStudentListRightSideBar;

        public MainViewModel()
        {
            GotoLoginViewCommand = new RelayCommand<object>((p) => true, (p) => GotoLoginView());
            GotoLayoutViewCommand = new RelayCommand<object>
                (
                (p) =>
                {
                    if (string.IsNullOrEmpty(_loginViewModel.Password) || string.IsNullOrEmpty(_loginViewModel.Username))
                        return false;
                    return true;
                },
                (p) => GotoLayoutView());

            _loginViewModel = new LoginViewModel();
            _layoutViewModel = new LayoutViewModel
            {
                IsMainWindow = true
            };
            InitContentView();
            InitRightSideBar();

            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, _layoutViewModel, "School"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFacultyTrainingFormViewModel, _adminFacultyTrainingFormRightSideBar, _layoutViewModel, "School"),
                new NavigationItem("Môn học", false, null, _adminSubjectViewModel, _adminSubjectRightSideBar, _layoutViewModel, "School"),
            };
            ObservableCollection<NavigationItem> tempInfo = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Thông tin cá nhân", false, null, _settingUserInfoViewModel, null, _layoutViewModel, "AccountOutline"),
            };

            _layoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Tổng quan", false, null, _adminHomeViewModel, _adminHomeRightSideBar, _layoutViewModel, "ViewDashboardOutline"),
                new NavigationItem("Đào tạo", true, temp, null, null, _layoutViewModel, "SchoolOutline"),
                new NavigationItem("Đăng ký học phần", false, null, _studentCourseRegistryViewModel, _studentCourseRegistryRightSideBar, _layoutViewModel, "CreditCardPlusOutline"),
                new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, _layoutViewModel, "DiceD10Outline"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, _layoutViewModel, "BellOutline"),
                new NavigationItem("TKB", false, null, _studentScheduleTableViewModel, null, _layoutViewModel, "CalendarMonthOutline"),
                new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoViewModel, null, _layoutViewModel, "AccountCircleOutline"),
                new NavigationItem("Danh sách sinh viên", false, null, _adminStudentListViewModel, _studentListRightSideBar, _layoutViewModel, "SchoolOutline"),
                new NavigationItem("Danh sách sinh viên", false, null, _campusStudentListViewModel, _campusStudentListRightSideBar, _layoutViewModel, "SchoolOutline"),
                new NavigationItem("Admin - ĐKHP", false, null, _adminCourseRegistryViewModel, _adminCourseRegistryRightSideBar, _layoutViewModel, "CreditCardPlusOutline"),
                new NavigationItem("Cài đặt", true, tempInfo, null, null, _layoutViewModel, "CogOutline"),
            };

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

        private void GotoLayoutView()
        {
            if (_loginViewModel.IsExistAccount())
            {
                CurrentViewModel = _layoutViewModel;
            }
        }

        private void GotoLoginView()
        {
            CurrentViewModel = _loginViewModel;
        }

        public void InitContentView()
        {
            _adminHomeViewModel = new AdminHomeViewModel();

            _adminSubjectClassViewModel = new AdminSubjectClassViewModel();

            _adminSubjectViewModel = new AdminSubjectViewModel();

            _adminNotificationViewModel = new AdminNotificationViewModel();

            _studentCourseRegistryViewModel = new StudentCourseRegistryViewModel();

            _adminFacultyTrainingFormViewModel = new AdminFacultyTrainingFormViewModel();

            _scoreboardViewModel = new ScoreBoardViewModel();

            _studentScheduleTableViewModel = new StudentScheduleTableViewModel();

            _adminUserInfoViewModel = new UserInfoViewModel();

            _adminCourseRegistryViewModel = new AdminCourseRegistryViewModel();

            _adminStudentListViewModel = new AdminStudentListViewModel();

            _campusStudentListViewModel = new CampusStudentListViewModel();

            _settingUserInfoViewModel = new SettingUserInfoViewModel();

            // Set default view
            _layoutViewModel.ContentViewModel = _adminHomeViewModel;
        }

        public void InitRightSideBar()
        {
            _adminHomeRightSideBar = new AdminHomeRightSideBarViewModel();

            _adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBarViewModel();

            _adminSubjectRightSideBar = new AdminSubjectRightSideBarViewModel();

            _adminNotificationRightSideBar = new AdminNotificationRightSideBarViewModel();

            _studentCourseRegistryRightSideBar = new StudentCourseRegistryRightSideBarViewModel();

            _adminFacultyTrainingFormRightSideBar = new AdminFacultyTrainingFormRightSideBarViewModel();

            _adminCourseRegistryRightSideBar = new AdminCourseRegistryRightSideBarViewModel();

            _scoreboardRightSideBar = new ScoreBoardRightSideBarViewModel(); //ViewModel

            _studentListRightSideBar = new StudentListRightSideBarViewModel();

            _campusStudentListRightSideBar = new CampusStudentListRightSideBarViewModel();

            _layoutViewModel.RightSideBar = _adminHomeRightSideBar;
        }
    }
}