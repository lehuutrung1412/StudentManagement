using StudentManagement.Objects;
using System.Collections.ObjectModel;

namespace StudentManagement.ViewModels
{
    public class LayoutViewModel : BaseViewModel
    {
        // current contentViewModel and rightSideBarViewModel
        private object _contentViewModel;
        private object _rightSideBar;

        public object ContentViewModel
        {
            get => _contentViewModel;
            set
            {
                _contentViewModel = value;
                OnPropertyChanged();
            }
        }

        public object RightSideBar
        {
            get => _rightSideBar;
            set
            {
                _rightSideBar = value;
                OnPropertyChanged();
            }
        }

        private bool _isMainWindow;

        public bool IsMainWindow
        {
            get { return _isMainWindow; }
            set { _isMainWindow = value; OnPropertyChanged(); }
        }

        public ObservableCollection<NavigationItem> _navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems { get => _navigationItems; set => _navigationItems = value; }
    }
}
