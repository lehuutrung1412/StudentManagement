using StudentManagement.Commands;
using StudentManagement.Components;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using static StudentManagement.Services.LoginServices;
using NavigationItem = StudentManagement.Objects.NavigationItem;

namespace StudentManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static MainViewModel s_instance;
        public static MainViewModel Instance
        {
            get => s_instance ?? (s_instance = new MainViewModel());

            private set => s_instance = value;
        }

        private object _currentViewModel;
        private LoginViewModel _loginViewModel;
        private LayoutViewModel _layoutViewModel;
        public LayoutViewModel LayoutViewModel { get => _layoutViewModel; set => _layoutViewModel = value; }

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

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
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
        private object _campusStudentListRightSideBar;

        public MainViewModel()
        {
            Instance = this;

            LoginServices.UpdateCurrentUser += UpdateCurrentUser;

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
            LayoutViewModel = new LayoutViewModel
            {
                IsMainWindow = true
            };
            InitContentView();
            InitRightSideBar();

            #region init all navigations
            //ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
            //    new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, _layoutViewModel, "School"),
            //    new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFacultyTrainingFormViewModel, _adminFacultyTrainingFormRightSideBar, _layoutViewModel, "School"),
            //    new NavigationItem("Môn học", false, null, _adminSubjectViewModel, _adminSubjectRightSideBar, _layoutViewModel, "School"),
            //};
            //ObservableCollection<NavigationItem> tempInfo = new ObservableCollection<NavigationItem>() {
            //    new NavigationItem("Thông tin cá nhân", false, null, _settingUserInfoViewModel, null, _layoutViewModel, "AccountOutline"),
            //};

            //_layoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
            //    new NavigationItem("Tổng quan", false, null, _adminHomeViewModel, _adminHomeRightSideBar, _layoutViewModel, "ViewDashboardOutline"),
            //    new NavigationItem("Đào tạo", true, temp, null, null, _layoutViewModel, "SchoolOutline"),
            //    new NavigationItem("Đăng ký học phần", false, null, _studentCourseRegistryViewModel, _studentCourseRegistryRightSideBar, _layoutViewModel, "CreditCardPlusOutline"),
            //    new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, _layoutViewModel, "DiceD10Outline"),
            //    new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, _layoutViewModel, "BellOutline"),
            //    new NavigationItem("TKB", false, null, _studentScheduleTableViewModel, null, _layoutViewModel, "CalendarMonthOutline"),
            //    new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoViewModel, null, _layoutViewModel, "AccountCircleOutline"),
            //    new NavigationItem("Danh sách sinh viên", false, null, _adminStudentListViewModel, _studentListRightSideBar, _layoutViewModel, "SchoolOutline"),
            //    new NavigationItem("Danh sách sinh viên", false, null, _campusStudentListViewModel, _campusStudentListRightSideBar, _layoutViewModel, "SchoolOutline"),
            //    new NavigationItem("Admin - ĐKHP", false, null, _adminCourseRegistryViewModel, _adminCourseRegistryRightSideBar, _layoutViewModel, "CreditCardPlusOutline"),
            //    new NavigationItem("Cài đặt", true, tempInfo, null, null, _layoutViewModel, "CogOutline"),
            //};

            // Set corresponding active button to default view
            //ObservableCollection<NavigationItem> navigationItems = _layoutViewModel.NavigationItems;
            //foreach (var item in navigationItems)
            //{
            //    if (item.NavigationItemViewModel == _layoutViewModel.ContentViewModel)
            //    {
            //        item.IsPressed = true;
            //        break;
            //    }
            //}
            #endregion

            //CurrentViewModel = _layoutViewModel;
            CurrentViewModel = _loginViewModel;

            MainWindow.Notify.ShowBalloonTip(3000, "Stuman - Hệ thống quản lý đào tạo", "Chào mừng bạn đến với Stuman", System.Windows.Forms.ToolTipIcon.Info);
        }

        #region methods
        public void InitNavigationItemsByRole()
        {
            LayoutViewModel.ContentViewModel = _adminHomeViewModel;
            LayoutViewModel.RightSideBar = _adminHomeRightSideBar;

            switch (LoginServices.CurrentUser.UserRole.Role)
            {
                case "Admin":
                    InitAdminNavigationItems();
                    break;
                case "Giáo viên":
                    InitTeacherNavigationItems();
                    break;
                default:
                    InitStudentNavigationItems();
                    break;
            }

            // Set corresponding active button to default view
            ObservableCollection<NavigationItem> navigationItems = LayoutViewModel.NavigationItems;
            foreach (var item in navigationItems)
            {
                if (item.NavigationItemViewModel == LayoutViewModel.ContentViewModel)
                {
                    item.IsPressed = true;
                    break;
                }
            }
        }

        public void InitAdminNavigationItems()
        {
            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, LayoutViewModel, "GoogleClassroom"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFacultyTrainingFormViewModel, _adminFacultyTrainingFormRightSideBar, LayoutViewModel, "Domain"),
                new NavigationItem("Môn học", false, null, _adminSubjectViewModel, _adminSubjectRightSideBar, LayoutViewModel, "BookEducationOutline"),
            };
            ObservableCollection<NavigationItem> tempInfo = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Thông tin cá nhân", false, null, _settingUserInfoViewModel, null, LayoutViewModel, "AccountOutline"),
            };

            LayoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Tổng quan", false, null, _adminHomeViewModel, _adminHomeRightSideBar, LayoutViewModel, "ViewDashboardOutline"),
                new NavigationItem("Đào tạo", true, temp, null, null, LayoutViewModel, "SchoolOutline"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, LayoutViewModel, "BellOutline"),
                new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoViewModel, null, LayoutViewModel, "AccountCircleOutline"),
                new NavigationItem("Người dùng hệ thống", false, null, _campusStudentListViewModel, _campusStudentListRightSideBar, LayoutViewModel, "AccountGroupOutline"),
                new NavigationItem("Đăng ký học phần", false, null, _adminCourseRegistryViewModel, _adminCourseRegistryRightSideBar, LayoutViewModel, "CreditCardPlusOutline"),
                new NavigationItem("Cài đặt", true, tempInfo, null, null, LayoutViewModel, "CogOutline"),
            };
        }

        public void InitTeacherNavigationItems()
        {
            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, LayoutViewModel, "GoogleClassroom"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFacultyTrainingFormViewModel, _adminFacultyTrainingFormRightSideBar, LayoutViewModel, "Domain"),
                new NavigationItem("Môn học", false, null, _adminSubjectViewModel, _adminSubjectRightSideBar, LayoutViewModel, "BookEducationOutline"),
            };

            LayoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Tổng quan", false, null, _adminHomeViewModel, _adminHomeRightSideBar, LayoutViewModel, "ViewDashboardOutline"),
                new NavigationItem("Đào tạo", true, temp, null, null, LayoutViewModel, "SchoolOutline"),
                new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, LayoutViewModel, "DiceD10Outline"),
                new NavigationItem("Thời khóa biểu", false, null, _studentScheduleTableViewModel, null, LayoutViewModel, "CalendarMonthOutline"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, LayoutViewModel, "BellOutline"),
                new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoViewModel, null, LayoutViewModel, "AccountCircleOutline")
            };
        }

        public void InitStudentNavigationItems()
        {
            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, LayoutViewModel, "GoogleClassroom"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFacultyTrainingFormViewModel, _adminFacultyTrainingFormRightSideBar, LayoutViewModel, "Domain"),
                new NavigationItem("Môn học", false, null, _adminSubjectViewModel, _adminSubjectRightSideBar, LayoutViewModel, "BookEducationOutline"),
            };

            LayoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Tổng quan", false, null, _adminHomeViewModel, _adminHomeRightSideBar, LayoutViewModel, "ViewDashboardOutline"),
                new NavigationItem("Đào tạo", true, temp, null, null, LayoutViewModel, "SchoolOutline"),
                new NavigationItem("Đăng ký học phần", false, null, _studentCourseRegistryViewModel, _studentCourseRegistryRightSideBar, LayoutViewModel, "CreditCardPlusOutline"),
                new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, LayoutViewModel, "DiceD10Outline"),
                new NavigationItem("Thời khóa biểu", false, null, _studentScheduleTableViewModel, null, LayoutViewModel, "CalendarMonthOutline"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, LayoutViewModel, "BellOutline"),
                new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoViewModel, null, LayoutViewModel, "AccountCircleOutline"),
            };
        }

        private void GotoLayoutView()
        {
            if (_loginViewModel.IsExistAccount())
            {
                CurrentViewModel = LayoutViewModel;
            }
        }

        private void GotoLoginView()
        {
            CloseAllOtherWindows();
            CurrentViewModel = _loginViewModel;
        }

        private void CloseAllOtherWindows()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }
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

            _campusStudentListViewModel = new CampusStudentListViewModel();

            _settingUserInfoViewModel = new SettingUserInfoViewModel();

            // Set default view
            LayoutViewModel.ContentViewModel = _adminHomeViewModel;
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

            _scoreboardRightSideBar = new ScoreBoardRightSideBarViewModel();

            _campusStudentListRightSideBar = new CampusStudentListRightSideBarViewModel();

            LayoutViewModel.RightSideBar = _adminHomeRightSideBar;
        }

        #endregion

        #region eventhandlers
        private void UpdateCurrentUser(object sender, LoginEvent e)
        {
            InitNavigationItemsByRole();
        }
        #endregion
    }
}