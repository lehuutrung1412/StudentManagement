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
        private object _contentViewModel;
        private object _rightSideBar;
        private object _adminHomeViewModel;
        private object _adminSubjectClassViewModel;
        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;

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

        public LayoutViewModel()
        {
            InitContentView();

            InitRightSideBar();

            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminHomeView());
            GotoAdminSubjectClassViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminSubjectClassView());

            //NavigationItems = new ObservableCollection<NavigationItem>() {
            //    new NavigationItem("Trang chủ", false, null, ),
            //};
            //NavigationItem(string navigationHeader, bool canBeExpanded, List < NavigationItem > expandedItems, object navigationItemView, LayoutViewModel layoutViewModel)
        }

        public void InitContentView()
        {
            this._adminHomeViewModel = new AdminHomeViewModel();

            this._adminSubjectClassViewModel = new AdminSubjectClassViewModel();

            this.ContentViewModel = this._adminHomeViewModel;
        }

        public void InitRightSideBar()
        {
            this._adminHomeRightSideBar = new AdminHomeRightSideBar();

            this._adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBarViewModel();

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

        public class NavigationItem
        {
            private string _navigationHeader;

            private bool _canBeExpanded;

            private List<NavigationItem> _expandedItems;

            private object _navigationItemViewModel;

            private object _rightSideBarNavigationItemViewModel;

            private ICommand _goToView;

            private static LayoutViewModel layoutViewModel;

            public NavigationItem(string navigationHeader, bool canBeExpanded, List<NavigationItem> expandedItems, object navigationItemViewModel, LayoutViewModel layoutViewModel)
            {
                _navigationHeader = navigationHeader;
                _canBeExpanded = canBeExpanded;
                _expandedItems = expandedItems;
                _navigationItemViewModel = navigationItemViewModel;
                LayoutViewModel = layoutViewModel;
                GoToView = new RelayCommand<object>((p) => { return true; }, (p) => GoToViewFunction());
            }

            public string NavigationHeader { get => _navigationHeader; set => _navigationHeader = value; }
            public bool CanBeExpanded { get => _canBeExpanded; set => _canBeExpanded = value; }
            public List<NavigationItem> ExpandedItems { get => _expandedItems; set => _expandedItems = value; }
            public object NavigationItemViewModel { get => _navigationItemViewModel; set => _navigationItemViewModel = value; }
            public object RightSideBarNavigationItemViewModel { get => _rightSideBarNavigationItemViewModel; set => _rightSideBarNavigationItemViewModel = value; }
            public ICommand GoToView { get => _goToView; set => _goToView = value; }
            public static LayoutViewModel LayoutViewModel { get => layoutViewModel; set => layoutViewModel = value; }

            private void GoToViewFunction()
            {
                LayoutViewModel.ContentViewModel = this.NavigationItemViewModel;
                LayoutViewModel.RightSideBar = this.RightSideBarNavigationItemViewModel;

            }
        }
    }
}
