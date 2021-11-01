using StudentManagement.Commands;
using StudentManagement.Components;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
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
        private object _contentView;
        private object _rightSideBar;
        private object _adminHomeView;
        private object _adminSubjectClassView;
        private object _adminNotificationView;
        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;
        private object _adminNotificationRightSideBar;

        public object ContentView
        {
            get { return _contentView; }
            set
            {
                _contentView = value;
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
        public ICommand GotoAdminNotificationCommand { get => _gotoAdminNotificationCommand; set => _gotoAdminNotificationCommand = value; }

        public LayoutViewModel()
        {
            InitContentView();

            InitRightSideBar();

            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminHomeView());
            GotoAdminSubjectClassViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminSubjectClassView());
            GotoAdminNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminNotificationView());
        }

        public void InitContentView()
        {
            this._adminHomeView = new AdminHome();

            this._adminSubjectClassView = new AdminSubjectClass();


            this._adminNotificationView = new AdminNotification();

            this.ContentView = this._adminHomeView;
        }

        public void InitRightSideBar()
        {
            this._adminHomeRightSideBar = new AdminHomeRightSideBar();

            this._adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBar();

            this._adminNotificationRightSideBar = new AdminNotificationRightSideBar();

            this.RightSideBar = this._adminHomeRightSideBar;
        }

        private void GotoAdminHomeView()
        {
            this.ContentView = this._adminHomeView;
            this.RightSideBar = this._adminHomeRightSideBar;

        }

        private void GotoAdminSubjectClassView()
        {
            this.ContentView = this._adminSubjectClassView;
            this.RightSideBar = this._adminSubjectClassRightSideBar;
        }

        private void GotoAdminNotificationView()
        {
            this.ContentView = this._adminNotificationView;
            this.RightSideBar = this._adminNotificationRightSideBar;
        }
    }
}
