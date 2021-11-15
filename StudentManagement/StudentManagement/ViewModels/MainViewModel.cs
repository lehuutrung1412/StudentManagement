using StudentManagement.Commands;
using StudentManagement.Objects;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentViewModel;
        private object _loginViewModel;
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
        private object _adminNotificationViewModel;
        private object _newFeedSubjectClassDetailViewModel;
        private object _studentCourseRegistryViewModel;
        private object _adminFalcutyTrainingFormViewModel;
        private object _scoreboardViewModel;
        private object _studentScheduleTableViewModel;
        private object _adminUserInfoStudentViewModel;

        // Rightsidebar corresponding to _contentViewModel
        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;
        private object _adminNotificationRightSideBar;
        private object _studentCourseRegistryRightSideBar;
        private object _adminFalcutyTrainingFormRightSideBar;
        private object _scoreboardRightSideBar;

        public MainViewModel()
        {
            _loginViewModel = new LoginViewModel();
            _layoutViewModel = new LayoutViewModel();

            InitContentView();
            InitRightSideBar();

            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, _layoutViewModel, "School"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFalcutyTrainingFormViewModel, _adminFalcutyTrainingFormRightSideBar, _layoutViewModel, "School")
            };

            _layoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Trang chủ", false, null, _adminHomeViewModel, _adminHomeRightSideBar, _layoutViewModel, "Home"),
                new NavigationItem("Lớp môn học", false, null, _newFeedSubjectClassDetailViewModel, null, _layoutViewModel, "Home"),
                new NavigationItem("Đào tạo", true, temp, null, null, _layoutViewModel, "ClockOutline"),
                new NavigationItem("Đăng ký học phần", false, null, _studentCourseRegistryViewModel, _studentCourseRegistryRightSideBar, _layoutViewModel, "CreditCardPlusOutline"),
                new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, _layoutViewModel, "Cat"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, _layoutViewModel, "Home"),
                new NavigationItem("TKB", false, null, _studentScheduleTableViewModel, null, _layoutViewModel, "Home"),
                new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoStudentViewModel, null, _layoutViewModel, "AccountOutline")
            };

            GotoLoginViewCommand = new RelayCommand<object>((p) => true, (p) => GotoLoginView());
            GotoLayoutViewCommand = new RelayCommand<object>((p) => true, (p) => GotoLayoutView());

            CurrentViewModel = _loginViewModel;
        }

        private void GotoLayoutView()
        {
            if ((_loginViewModel as LoginViewModel).IsExistAccount())
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

            _adminNotificationViewModel = new AdminNotificationViewModel();

            _newFeedSubjectClassDetailViewModel = new NewFeedSubjectClassDetailViewModel();

            _studentCourseRegistryViewModel = new StudentCourseRegistryViewModel();

            _adminFalcutyTrainingFormViewModel = new AdminFalcutyTrainingFormViewModel();

            _studentCourseRegistryViewModel = new StudentCourseRegistryViewModel();

            _scoreboardViewModel = new ScoreBoardViewModel();

            _studentScheduleTableViewModel = new StudentScheduleTableViewModel();

            _adminUserInfoStudentViewModel = new UserInfoStudentViewModel();

            _layoutViewModel.ContentViewModel = _adminHomeViewModel;
        }

        public void InitRightSideBar()
        {
            _adminHomeRightSideBar = new AdminHomeRightSideBarViewModel();

            _adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBarViewModel();

            _adminNotificationRightSideBar = new AdminNotificationRightSideBarViewModel();

            _studentCourseRegistryRightSideBar = new StudentCourseRegistryRightSideBarViewModel();

            _adminFalcutyTrainingFormRightSideBar = new AdminFalcutyTrainingFormRightSideBarViewModel();

            _scoreboardRightSideBar = new ScoreBoardRightSideBarViewModel(); //ViewModel

            _layoutViewModel.RightSideBar = _adminHomeRightSideBar;
        }
    }
}