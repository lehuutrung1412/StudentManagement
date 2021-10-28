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
        private ICommand _gotoAdminSubjectClassViewCommand;
        private object _contentView;
        private object _adminHomeView;
        private object _adminSubjectClassView;

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
        public ICommand GotoAdminSubjectClassViewCommand { get => _gotoAdminSubjectClassViewCommand; set => _gotoAdminSubjectClassViewCommand = value; }

        public LayoutViewModel()
        {
            this._adminHomeView = new AdminHome();

            this._adminSubjectClassView = new AdminSubjectClass();

            ContentView = _adminHomeView;

            GotoAdminHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminHomeView());
            GotoAdminSubjectClassViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => GotoAdminSubjectClassView());
        }

        private void GotoAdminHomeView()
        {
            ContentView = _adminHomeView;
        }

        private void GotoAdminSubjectClassView()
        {
            ContentView = _adminSubjectClassView;
        }
    }
}
