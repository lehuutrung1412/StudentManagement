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
        private ICommand _gotoStudentCourseRegistryViewCommand;
        private object _contentView;
        private object _rightSideBar;
        private object _adminHomeView;
        private object _adminSubjectClassView;
        private object _studentCourseRegistryView;
        private object _adminHomeRightSideBar;
        private object _adminSubjectClassRightSideBar;
        private object _studentCourseRegistryRightSideBar;

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
        public ICommand GotoStudentCourseRegistryViewCommand { get => _gotoStudentCourseRegistryViewCommand; set => _gotoStudentCourseRegistryViewCommand = value; }
        public ICommand GotoAdminSubjectClassViewCommand { get => _gotoAdminSubjectClassViewCommand; set => _gotoAdminSubjectClassViewCommand = value; }


        public LayoutViewModel()
        {
            InitContentView();

            InitRightSideBar();

            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminHomeView());
            GotoAdminSubjectClassViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminSubjectClassView());
            GotoStudentCourseRegistryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoStudentCourseRegistryView());
        }

        public void InitContentView()
        {
            this._adminHomeView = new AdminHome();

            this._adminSubjectClassView = new AdminSubjectClass();
            this._studentCourseRegistryView = new StudentCourseRegistry();

            this.ContentView = this._adminHomeView;
        }

        public void InitRightSideBar()
        {
            this._adminHomeRightSideBar = new AdminHomeRightSideBar();

            this._adminSubjectClassRightSideBar = new AdminSubjectClassRightSideBar();
            this._studentCourseRegistryRightSideBar = new StudentCourseRegistryRightSideBar();

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
        private void GotoStudentCourseRegistryView()
        {
            this.ContentView = this._studentCourseRegistryView;
            this.RightSideBar = this._studentCourseRegistryRightSideBar;
        }
    }
}
