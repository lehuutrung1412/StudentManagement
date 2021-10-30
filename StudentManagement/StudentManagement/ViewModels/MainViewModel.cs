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
        private object _infoStudentViewModel;

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
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

            _infoStudentViewModel = new UserInfoStudentViewModel();

            CurrentViewModel = _loginViewModel;

            GotoLoginViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoLoginView());
            GotoLayoutViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoLayoutView());
        }

        private void GotoLayoutView()
        {
            CurrentViewModel = _LayoutViewModel;
        }

        private void GotoLoginView()
        {
            //MyMessageBox.Show("Lorem Ipsum is simply dummy text of the printing and typesetting industry.", "ABC", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            //MyMessageBox.Show("Lorem Ipsum is simply dummy text of the printing and typesetting industry.", "ABC", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            //MyMessageBox.Show("Lorem Ipsum is simply dummy text of the printing and typesetting industry.", "ABC", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            //MyMessageBox.Show("Lorem Ipsum is simply dummy text of the printing and typesetting industry.", "ABC", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            // CurrentViewModel = _loginViewModel;
            CurrentViewModel = _infoStudentViewModel;
        }
    }
}
