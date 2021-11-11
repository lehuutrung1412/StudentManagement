using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ICommand _gotoLoginViewCommand;
        private ICommand _gotoLayoutViewCommand;
        private object _currentViewModel;
        private object _loginViewModel;
        private object _LayoutViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand GotoLoginViewCommand { get => _gotoLoginViewCommand; set => _gotoLoginViewCommand = value; }
        public ICommand GotoLayoutViewCommand { get => _gotoLayoutViewCommand; set => _gotoLayoutViewCommand = value; }

        public MainViewModel()
        {
            _loginViewModel = new LoginViewModel();

            _LayoutViewModel = new LayoutViewModel();

            CurrentViewModel = _loginViewModel;

            GotoLoginViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoLoginView());
            GotoLayoutViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoLayoutView());
        }

        private void GotoLayoutView()
        {
            if ((_loginViewModel as LoginViewModel).IsExistAccount())
            {
                CurrentViewModel = _LayoutViewModel;
            }
        }

        private void GotoLoginView()
        {
            CurrentViewModel = _loginViewModel;
        }
    }
}
