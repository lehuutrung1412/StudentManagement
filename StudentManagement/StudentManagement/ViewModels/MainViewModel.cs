﻿using StudentManagement.Commands;
using StudentManagement.Components;
using StudentManagement.Objects;
using StudentManagement.ViewModels.CourseRegistry;
using StudentManagement.ViewModels.Dashboard;
using StudentManagement.ViewModels.FalcutyTrainingForm;
using StudentManagement.ViewModels.Notification;
using StudentManagement.ViewModels.ScheduleTable;
using StudentManagement.ViewModels.ScoreBoard;
using StudentManagement.ViewModels.SubjectClass;
using StudentManagement.ViewModels.UserInfo;
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
        private object _adminNotificationViewModel;
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
            GotoLoginViewCommand = new RelayCommand<object>((p) => true, (p) => GotoLoginView());
            GotoLayoutViewCommand = new RelayCommand<object>((p) => true, (p) => GotoLayoutView());

            _loginViewModel = new LoginViewModel();
            _layoutViewModel = new LayoutViewModel
            {
                IsMainWindow = true
            };
            InitContentView();
            InitRightSideBar();

            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, _layoutViewModel, "School"),
                new NavigationItem("Khoa - hệ đào tạo", false, null, _adminFalcutyTrainingFormViewModel, _adminFalcutyTrainingFormRightSideBar, _layoutViewModel, "School")
            };

            _layoutViewModel.NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Trang chủ", false, null, _adminHomeViewModel, _adminHomeRightSideBar, _layoutViewModel, "Home"),
                new NavigationItem("Đào tạo", true, temp, null, null, _layoutViewModel, "ClockOutline"),
                new NavigationItem("Đăng ký học phần", false, null, _studentCourseRegistryViewModel, _studentCourseRegistryRightSideBar, _layoutViewModel, "CreditCardPlusOutline"),
                new NavigationItem("Bảng điểm sinh viên", false, null, _scoreboardViewModel, _scoreboardRightSideBar, _layoutViewModel, "Cat"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, _layoutViewModel, "Home"),
                new NavigationItem("TKB", false, null, _studentScheduleTableViewModel, null, _layoutViewModel, "Home"),
                new NavigationItem("Thông tin cá nhân", false, null, _adminUserInfoStudentViewModel, null, _layoutViewModel, "AccountOutline")
            };
            
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

            _adminNotificationViewModel = new AdminNotificationViewModel();

            _studentCourseRegistryViewModel = new StudentCourseRegistryViewModel();

            _adminFalcutyTrainingFormViewModel = new AdminFalcutyTrainingFormViewModel();

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