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
    public class LayoutViewModel : BaseViewModel
    {
        private ICommand _gotoAdminHomeViewCommand;
        private object _contentView;
        private object _adminHomeView;

        public object ContentView
        {
            get { return _contentView; }
            set
            {
                _contentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand GotoAdminHomeViewCommand { get => _gotoAdminHomeViewCommand; set => _gotoAdminHomeViewCommand = value; }

        public LayoutViewModel()
        {
            _adminHomeView = new AdminHome();

            ContentView = _adminHomeView;

            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoLoginView());
        }

        private void GotoLoginView()
        {
            ContentView = _adminHomeView;
        }
    }
}
