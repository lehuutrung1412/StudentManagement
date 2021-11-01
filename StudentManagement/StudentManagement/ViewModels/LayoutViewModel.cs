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
        private ICommand _gotoAdminHomeViewCommand;
        private ICommand _gotoAdminSubjectClassViewCommand;
        private ICommand _gotoAdminNotificationCommand;


        private object _contentViewModel;
        private object _rightSideBar;
        private object _adminHomeViewModel;
        private object _adminSubjectClassViewModel;
        private object _adminNotificationViewModel;


        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;
        private object _adminNotificationRightSideBar;

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

        public ICommand GotoAdminHomeViewCommand { get => _gotoAdminHomeViewCommand; set => _gotoAdminHomeViewCommand = value; }
        public ICommand GotoAdminSubjectClassViewCommand { get => _gotoAdminSubjectClassViewCommand; set => _gotoAdminSubjectClassViewCommand = value; }

        public ObservableCollection<NavigationItem> _navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems { get => _navigationItems; set => _navigationItems = value; }
        public ICommand GotoAdminNotificationCommand { get => _gotoAdminNotificationCommand; set => _gotoAdminNotificationCommand = value; }

        public LayoutViewModel()
        {
            InitContentView();

            InitRightSideBar();

            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminHomeView());
            GotoAdminSubjectClassViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminSubjectClassView());

            ObservableCollection<NavigationItem> temp = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Lớp môn học", false, null, _adminSubjectClassViewModel, _adminSubjectClassRightSideBar, this, "School"),
            };

            NavigationItems = new ObservableCollection<NavigationItem>() {
                new NavigationItem("Trang chủ", false, null, _adminHomeViewModel, _adminHomeRightSideBar, this, "Home"),
                new NavigationItem("Trang chủ", false, null, _adminHomeViewModel, _adminHomeRightSideBar, this, "Home"),
                new NavigationItem("Đào tạo", true, temp, null, null, this, "ClockOutline"),
                new NavigationItem("Thông báo", false, null, _adminNotificationViewModel, _adminNotificationRightSideBar, this, "Home"),
            };

            GotoAdminNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminNotificationView());
        }

        public void InitContentView()
        {
            this._adminHomeViewModel = new AdminHomeViewModel();

            this._adminSubjectClassViewModel = new AdminSubjectClassViewModel();

            this._adminNotificationViewModel = new AdminNotificationViewModel();

            this.ContentViewModel = this._adminHomeViewModel;
        }

        public void InitRightSideBar()
        {
            this._adminHomeRightSideBar = new AdminHomeRightSideBar();

            this._adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBarViewModel();

            this._adminNotificationRightSideBar = new AdminNotificationRightSideBar();

            this.RightSideBar = this._adminHomeRightSideBar;
        }

        private void GotoAdminHomeView()
        {
            this.ContentViewModel = this._adminHomeViewModel;
            this.RightSideBar = this._adminHomeRightSideBar;

        }

        private void GotoAdminSubjectClassView()
        {
            this.ContentViewModel = this._adminSubjectClassViewModel;
            this.RightSideBar = this._adminSubjectClassRightSideBar;
        }

        private void GotoAdminNotificationView()
        {
            this.ContentViewModel = this._adminNotificationViewModel;
            this.RightSideBar = this._adminNotificationRightSideBar;
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
