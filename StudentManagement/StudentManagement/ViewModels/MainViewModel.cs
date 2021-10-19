using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand _gotoLoginViewCommand;
        private ICommand _gotoAdminHomeViewCommand;
        private object _currentView;
        private object _loginView;
        private object _adminHomeView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand GotoLoginViewCommand { get => _gotoLoginViewCommand; set => _gotoLoginViewCommand = value; }
        public ICommand GotoAdminHomeViewCommand { get => _gotoAdminHomeViewCommand; set => _gotoAdminHomeViewCommand = value; }

        public MainViewModel()
        {
            _loginView = new Login();
            _adminHomeView = new AdminHome();

            CurrentView = _loginView;

            GotoLoginViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoLoginView());
            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminHomeView());
        }

        private void GotoAdminHomeView()
        {
            CurrentView = _adminHomeView;
        }

        private void GotoLoginView()
        {
            CurrentView = _loginView;
        }
    }
}
